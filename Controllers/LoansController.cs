using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    public class LoansController : Controller
    {
        private readonly LoanRepository _loanRepository;
        private readonly BookRepository _bookRepository;
        private readonly MemberRepository _memberRepository;

        public LoansController(LoanRepository loanRepository, BookRepository bookRepository, MemberRepository memberRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
        }

        public async Task<IActionResult> Index()
        {
            var loans = await _loanRepository.GetActiveLoansAsync();
            var books = await _bookRepository.GetAllAsync();
            var members = await _memberRepository.GetAllAsync();

            var loanViewModels = loans.Select(loan => new Models.LoanViewModel
            {
                Id = loan.Id,
                BookTitle = books.FirstOrDefault(b => b.Id == loan.BookId)?.Title ?? "Unknown",
                MemberName = members.FirstOrDefault(m => m.Id == loan.MemberId)?.FullName ?? "Unknown",
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate
            }).ToList();

            return View(loanViewModels);
        }

        public async Task<IActionResult> Issue()
        {
            ViewBag.Books = (await _bookRepository.GetAllAsync()).Where(b => b.AvailableCopies > 0).ToList();
            ViewBag.Members = await _memberRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Issue(string bookId, string memberId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null || book.AvailableCopies < 1)
            {
                TempData["Error"] = "Book not available.";
                return RedirectToAction(nameof(Issue));
            }
            var loan = new Loan
            {
                BookId = bookId,
                MemberId = memberId,
                LoanDate = DateTime.Now
            };
            await _loanRepository.AddAsync(loan);
            book.AvailableCopies--;
            await _bookRepository.UpdateAsync(bookId, book);
            TempData["Success"] = "Book issued successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Return(string id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null || loan.ReturnDate != null)
            {
                TempData["Error"] = "Invalid loan.";
                return RedirectToAction(nameof(Index));
            }
            return View(loan);
        }

        [HttpPost, ActionName("Return")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnConfirmed(string id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null || loan.ReturnDate != null)
            {
                TempData["Error"] = "Invalid loan.";
                return RedirectToAction(nameof(Index));
            }
            loan.ReturnDate = DateTime.Now;
            await _loanRepository.UpdateAsync(id, loan);
            var book = await _bookRepository.GetByIdAsync(loan.BookId);
            if (book != null)
            {
                book.AvailableCopies++;
                await _bookRepository.UpdateAsync(book.Id, book);
            }
            TempData["Success"] = "Book returned successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
} 