using ExpensiveControlApp.DTOs;
using ExpensiveControlApp.Models;
using ExpensiveControlApp.Models.Expensives;
using ExpensiveControlApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpensiveControlApp.Controllers
{
    public class ExpensiveController : Controller
    {
        private readonly ILogger<ExpensiveController> _logger;
        private readonly IExpensiveService _expensiveService;

        public ExpensiveController(ILogger<ExpensiveController> logger, IExpensiveService expensiveService)
        {
            _logger = logger;
            _expensiveService = expensiveService;
        }

        public async Task<IActionResult> Index()
        {
            var listExpensiveDto = new ListExpensiveDTO();
            try
            {
                listExpensiveDto.Items = await _expensiveService.FindByDate(listExpensiveDto.StartDate, listExpensiveDto.EndDate);
                listExpensiveDto.Count = listExpensiveDto.Items.Count;
                listExpensiveDto.Total = listExpensiveDto.VerifyTotal();
                return View(listExpensiveDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View(listExpensiveDto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ListExpensiveDTO listExpensiveDto)
        {
            try
            {
                listExpensiveDto.Items = await _expensiveService.FindByDate(listExpensiveDto.StartDate, listExpensiveDto.EndDate);
                listExpensiveDto.Count = listExpensiveDto.Items.Count;
                listExpensiveDto.Total = listExpensiveDto.VerifyTotal();
                return View(listExpensiveDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View(listExpensiveDto);
            }
        }


        public async Task<IActionResult> Create()
        {
            var createExpensiveDto = new CreateExpensiveDTO();
            try
            {
                return View(createExpensiveDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View(createExpensiveDto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateExpensiveDTO createExpensiveDto)
        {
            try
            {
                await _expensiveService.Create(createExpensiveDto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View(createExpensiveDto);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
                return NotFound();            
            try
            {
                var item = await _expensiveService.FindById(id);
                if (item == null)
                    return NotFound();
                return View(new UpdateExpensiveDTO
                {
                    Id = item.Id,
                    Description = item.Description,
                    Date = item.Date,
                    Value = item.Value
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateExpensiveDTO updateExpensiveDTO)
        {
            if(updateExpensiveDTO.Id == null || updateExpensiveDTO == null)
            {
                return BadRequest();
            }
            try
            {
                await _expensiveService.Update(updateExpensiveDTO);
                var item = await _expensiveService.FindById(updateExpensiveDTO.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View(updateExpensiveDTO);
            }
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            try
            {
                var item = await _expensiveService.FindById(id);
                if (item == null)
                    return NotFound();
                return View(item);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            
            try
            {
                await _expensiveService.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return RedirectToAction("Index");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}