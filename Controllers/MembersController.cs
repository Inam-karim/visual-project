using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Text.Json;

namespace LibraryManagementSystem.Controllers
{
    public class MembersController : Controller
    {
        private readonly MemberRepository _memberRepository;

        public MembersController(MemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IActionResult> Index()
        {
            var members = await _memberRepository.GetAllAsync();
            return View(members);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            // 🚨 FIX: Remove Id from validation
            ModelState.Remove("Id");

            Console.WriteLine($"[MembersController] ModelState.IsValid: {ModelState.IsValid}");
            Console.WriteLine($"[MembersController] Member: {JsonSerializer.Serialize(member)}");

            if (ModelState.IsValid)
            {
                try
                {
                    await _memberRepository.AddAsync(member);
                    TempData["Success"] = "Member added successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while adding the member: " + ex.Message);
                }
            }
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"[MembersController] Validation error for {key}: {error.ErrorMessage}");
                    }
                }
            }
            return View(member);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Member member)
        {
            if (ModelState.IsValid)
            {
                await _memberRepository.UpdateAsync(id, member);
                TempData["Success"] = "Member updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(string id)
        // {
        //     await _memberRepository.DeleteAsync(id);
        //     TempData["Success"] = "Member deleted successfully.";
        //     return RedirectToAction(nameof(Index));
        // }
        [HttpPost, ActionName("Delete")]  // Matches asp-action="Delete"
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(string id)
{
    await _memberRepository.DeleteAsync(id);
    TempData["Success"] = "Member deleted successfully.";
    return RedirectToAction(nameof(Index));
}
    }
}
