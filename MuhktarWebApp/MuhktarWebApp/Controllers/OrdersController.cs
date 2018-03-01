using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MuhktarWebApp.Data;
using MuhktarWebApp.Models.Classes;

namespace MuhktarWebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationContext _context;

        public OrdersController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Orders.Include(o => o.Category).Include(o => o.Route);
            foreach (var item in applicationContext.ToList())
            {
                if (item.Route.EndDate < DateTime.Now && item.State != State.Completed && item.State != State.Refunded && item.State != State.Canceled)
                {
                    RefundedData(item);
                }
            }
            return View(await applicationContext.ToListAsync());
        }

        public void RefundedData(Order order)
        {
            order.State = State.Refunded;
            _context.Update(order);
            _context.SaveChanges();
        }

        public async Task<IActionResult> CompleteData(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            order.State = State.Completed;
            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }
        public async Task<IActionResult> Manage()
        {
            var applicationDbContext = _context.Orders.Include(o => o.Category).Include(o => o.Route);
            return View(await applicationDbContext.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Accept(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            order.State = State.Awaiting;
            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            order.State = State.Awaiting;
            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            order.State = State.Canceled;
            order.CashBack = order.Price;
            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Category)
                .Include(o => o.Route)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            //ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "End");
            var RouteId = _context.Routes
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Start + " - " + s.End
                  });
            ViewData["RouteId"] = new SelectList(RouteId, "Value", "Text");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Customer,CustomerType,RouteId,CategoryId,State,Price,CashBack")] Order order)
        {
            if (ModelState.IsValid)
            {
                Category category = _context.Categories.Find(order.CategoryId);
                Route route = _context.Routes.Find(order.RouteId);
                TimeSpan span = (route.EndDate).Subtract(route.StartDate);
                var price = category.Price + route.Price * span.Hours;
                order.Price = price;
                order.CashBack = 0;
                order.State = State.Pending;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", order.CategoryId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "End", order.RouteId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", order.CategoryId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "End", order.RouteId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Customer,CustomerType,RouteId,CategoryId,State,Price,CashBack")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", order.CategoryId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "End", order.RouteId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Category)
                .Include(o => o.Route)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
