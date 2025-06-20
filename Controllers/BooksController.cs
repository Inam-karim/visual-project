using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Text.Json;

namespace LibraryManagementSystem.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookRepository _bookRepository;

        public BooksController(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.GetAllAsync();
            return View(books);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            Console.WriteLine($"[BooksController] ModelState.IsValid: {ModelState.IsValid}");
            Console.WriteLine($"[BooksController] Book: {JsonSerializer.Serialize(book)}");
            if (ModelState.IsValid)
            {
                try
                {
                    await _bookRepository.AddAsync(book);
                    TempData["Success"] = "Book added successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while adding the book: " + ex.Message);
                }
            }
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"[BooksController] Validation error for {key}: {error.ErrorMessage}");
                    }
                }
            }
            return View(book);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Book book)
        {
            if (ModelState.IsValid)
            {
                await _bookRepository.UpdateAsync(id, book);
                TempData["Success"] = "Book updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _bookRepository.DeleteAsync(id);
            TempData["Success"] = "Book deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
} 