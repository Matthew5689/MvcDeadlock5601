using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace mvcdeadlock5601.Controllers
{
    public class HomeController : Controller
    {
        static object object1 = new object();
        static object object2 = new object();

        public static void ObliviousFunction()
        {
            lock (object1)
            {
                Thread.Sleep(1000); // Wait for the blind to lead
                lock (object2)
                {
                }
            }
        }

        public static void BlindFunction()
        {
            lock (object2)
            {
                Thread.Sleep(1000); // Wait for oblivion
                lock (object1)
                {
                }
            }
        }

        static void MainMethod()
        {
            Thread thread1 = new Thread((ThreadStart)ObliviousFunction);
            Thread thread2 = new Thread((ThreadStart)BlindFunction);

            thread1.Start();
            thread2.Start();

            while (true)
            {
                // Stare at the two threads in deadlock.
            }
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            for (int x = 0; x < 10000; x++)
            {
                Task.Factory.StartNew(() => {
                    MainMethod();
                });
            }

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = GCSettings.IsServerGC;

            for (int x = 0; x < 10; x++)
            {
                byte[] memory = new byte[1000 * 1000 * 10]; // Ten million bytes
                memory[0] = 1; // Set memory (prevent allocation from being optimized out)
            }

            return View();
        }

        public ActionResult AnotherController()
        {
            ViewBag.Message = GCSettings.IsServerGC;

            for (int x = 0; x < 10; x++)
            {
                byte[] memory = new byte[1000 * 1000 * 10]; // Ten million bytes
                memory[0] = 1; // Set memory (prevent allocation from being optimized out)
            }

            return View();
        }
    }
}