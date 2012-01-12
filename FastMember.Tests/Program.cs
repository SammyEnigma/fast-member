﻿
using System;
using System.Diagnostics;
using System.ComponentModel;
namespace FastMember.Tests
{
    public class Program
    {
        public string Value { get; set; }
        static void Main()
        {
            var obj = new Program();
            const int loop = 5000000;
            string last = null;
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < loop; i++)
            {
                last = obj.Value;
                obj.Value = "abc";
            }
            watch.Stop();
            Console.WriteLine("Static C#: {0}ms", watch.ElapsedMilliseconds);

            dynamic dlr = obj;
            watch = Stopwatch.StartNew();
            for (int i = 0; i < loop; i++)
            {
                last = dlr.Value;
                dlr.Value = "abc";
            }
            watch.Stop();
            Console.WriteLine("Dynamic C#: {0}ms", watch.ElapsedMilliseconds);

            var prop = typeof (Program).GetProperty("Value");
            watch = Stopwatch.StartNew();
            for (int i = 0; i < loop; i++)
            {
                last = (string)prop.GetValue(obj, null);
                prop.SetValue(obj, "abc", null);
            }
            watch.Stop();
            Console.WriteLine("PropertyInfo: {0}ms", watch.ElapsedMilliseconds);

            var descriptor = TypeDescriptor.GetProperties(obj)["Value"];
            watch = Stopwatch.StartNew();
            for (int i = 0; i < loop; i++)
            {
                last = (string)descriptor.GetValue(obj);
                descriptor.SetValue(obj, "abc");
            }
            watch.Stop();
            Console.WriteLine("PropertyDescriptor: {0}ms", watch.ElapsedMilliseconds);

            Hyper.ComponentModel.HyperTypeDescriptionProvider.Add(typeof(Program));

            //descriptor = TypeDescriptor.GetProperties(obj)["Value"];
            //watch = Stopwatch.StartNew();
            //for (int i = 0; i < loop; i++)
            //{
            //    last = (string)descriptor.GetValue(obj);
            //    descriptor.SetValue(obj, "abc");
            //}
            //watch.Stop();
            //Console.WriteLine("HyperPropertyDescriptor: {0}ms", watch.ElapsedMilliseconds);

            var accessor = MemberAccess.GetAccessor(typeof (Program));
            watch = Stopwatch.StartNew();
            for (int i = 0; i < loop; i++)
            {
                last = (string)accessor[obj, "Value"];
                accessor[obj, "Value"] = "abc";
            }
            watch.Stop();
            Console.WriteLine("MemberAccess.GetAccessor: {0}ms", watch.ElapsedMilliseconds);

            var wrapped = MemberAccess.Wrap(obj);
            watch = Stopwatch.StartNew();
            for (int i = 0; i < loop; i++)
            {
                last = (string)wrapped["Value"];
                wrapped["Value"] = "abc";
            }
            watch.Stop();
            Console.WriteLine("MemberAccess.Wrap: {0}ms", watch.ElapsedMilliseconds);
            GC.KeepAlive(last);

        }
    }
}
