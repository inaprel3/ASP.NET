using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class MainController : Controller
{
    ApplicationContext db;
    public MainController(ApplicationContext context)
    {
        db = context;
        
    }

    public async Task<IActionResult> Index()
    {
        if (db.Users.ToList().Count == 0)
        {
            db.Users.Add(new User
            {
                FirstName = "Anastasiia",
                LastName = "Tsymbal",
                Age = 21
            });
            db.Users.Add(new User
            {
                FirstName = "Sam",
                LastName = "Winchester",
                Age = 47
            });
            db.Users.Add(new User
            {
                FirstName = "Merlin",
                LastName = "Monro",
                Age = 32
            });
        }

        if (db.Companies.ToList().Count == 0)
        {
            db.Companies.Add(new Company
            {
                Name = "Microsoft",
                Employees = 2000
            });
            db.Companies.Add(new Company
            {
                Name = "Apple",
                Employees = 2700
            });
            db.Companies.Add(new Company
            {
                Name = "Samsung",
                Employees = 1600
            });
        }

        await db.SaveChangesAsync();
        return View(new IndexModel
        {
            Users = await db.Users.ToListAsync(),
            Companies = await db.Companies.ToListAsync()
        });
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        Console.WriteLine(user.FirstName + " " + user.FirstName + " " + user.Age);
        db.Users.Add(user);

        await db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int? id)
    {

        if (id != null)
        {
            User? user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);

            if (user != null) return View(user);
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        db.Entry(user).State = EntityState.Modified;

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id != null)
        {
            User? user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
            if (user != null)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
        return NotFound();
    }
}
