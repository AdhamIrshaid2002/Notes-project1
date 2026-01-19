using Microsoft.AspNetCore.Mvc;
using project.Models;

public class NotesController : Controller
{
    private readonly AppDbContext _context;

    public NotesController(AppDbContext context)
    {
        _context = context;
    }

   
   

    public IActionResult Details(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var note = _context.Notes
            .FirstOrDefault(n => n.Id == id && n.UserId == userId);

        if (note == null)
            return NotFound();

        return View(note);
    }

    
    public IActionResult Create()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        return View();
    }

    
    [HttpPost]
    public IActionResult Create(Note note)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        note.UserId = userId.Value;

        _context.Notes.Add(note);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

  
    public IActionResult Edit(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var note = _context.Notes
            .FirstOrDefault(n => n.Id == id && n.UserId == userId);

        if (note == null)
            return NotFound();

        return View(note);
    }

    [HttpPost]
    public IActionResult Edit(Note updatedNote)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var note = _context.Notes
            .FirstOrDefault(n => n.Id == updatedNote.Id && n.UserId == userId);

        if (note == null)
            return NotFound();

        note.Title = updatedNote.Title;
        note.Content = updatedNote.Content;

        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    
    public IActionResult Delete(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var note = _context.Notes
            .FirstOrDefault(n => n.Id == id && n.UserId == userId);

        if (note == null)
            return NotFound();

        _context.Notes.Remove(note);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult Index(string search)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var notesQuery = _context.Notes
            .Where(n => n.UserId == userId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            notesQuery = notesQuery.Where(n => n.Title.Contains(search));
        }

        ViewBag.Search = search;

        return View(notesQuery.ToList());
    }



}
