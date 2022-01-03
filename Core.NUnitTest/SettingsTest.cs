using Core.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.NUnitTest
{
    public class SettingsTest
    {
        [Test, RequiresThread(System.Threading.ApartmentState.MTA)]
        public void MultiThreadTest() 
        {
            var settings1 = new List<Language>();
            var settings2 = new List<Language>();
            var t1 = Task.Run(() => { 
                settings1.AddRange(Settings.ActiveLanguages);
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId); 
            });
            var t2 = Task.Run(() => { 
                settings2.AddRange(Settings.ActiveLanguages);
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId); 
                 
            });
            Task.WaitAll(t1, t2);
            CollectionAssert.AreEqual(settings1, Settings.ActiveLanguages);
            CollectionAssert.AreEqual(settings2, Settings.ActiveLanguages);
        }
    }
}
