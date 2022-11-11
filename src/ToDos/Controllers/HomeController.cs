using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using ToDos.Models;
using ToDos.TaskDB;
using Task = ToDos.Models.Task;
using DBTask = ToDos.TaskDB.Task;

namespace ToDos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private List<Task> TaskList;

        public HomeController(ILogger<HomeController> logger)
        {
            TaskDBContext taskDBContext = new TaskDBContext();
            var taskData = taskDBContext.Tasks.ToList();
            TaskList = new List<Task>();
            foreach (var item in taskData)
            {
                TaskList.Add( new Task() {Id= int.Parse(item.Id),  Category = item.Category, Title = item.Title, Description = item.Description, IsActive = (bool)item.IsActive });
            }

            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(TaskList);
        }

        
        public IActionResult Update(int id)
        {
            var TaskId = id.ToString();
            TaskDBContext taskDBContext = new();
            var result = taskDBContext.Tasks.SingleOrDefault(b => b.Id == TaskId);
            if (result != null)
            {
                result.IsActive = false;
                taskDBContext.SaveChanges();
            }
            return RedirectToAction("Index","Home");
        }
        
        public IActionResult Delete(int id)
        {
            var TaskId = id.ToString();
            TaskDBContext taskDBContext = new();
            var result = taskDBContext.Tasks.SingleOrDefault(b => b.Id == TaskId);
            if (result != null)
            {
                taskDBContext.Tasks.Remove(result);
                taskDBContext.SaveChanges();
            }
            return RedirectToAction("Index","Home");
        }


        public IActionResult AddTask()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTask(Task obj)
        {
            ViewBag.formSubmitted = false;
            TaskDBContext taskDBContext = new();
            var id = TaskList.Count() +1 ;
            if (ModelState.IsValid)
            {
                var NewTask = new DBTask() {Id = id.ToString() ,  Category = obj.Category, Title = obj.Title, Description = obj.Description, IsActive = true };
                taskDBContext.Tasks.Add(NewTask);
                taskDBContext.SaveChanges();
                ViewBag.formSubmitted = true;   
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}