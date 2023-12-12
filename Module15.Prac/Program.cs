using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Module15.Prac
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Type myType = typeof(MyClass);
            Console.WriteLine($"Class Name: {myType.Name}\nConstructors: ");
            foreach(var ctor in myType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if(ctor.IsPublic)
                    Console.WriteLine($"\tPublic {ctor}");
                else Console.WriteLine($"\tPrivate {ctor}");
            }
            Console.WriteLine("Properties: ");
            foreach(var props in myType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)){
                if(props.GetGetMethod(true).IsPublic)
                    Console.WriteLine($"\tPublic {props}");
                else Console.WriteLine($"\tPrivate {props}");
            }
            Console.WriteLine("Methods: ");
            foreach(var meth in myType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (meth.IsPublic)
                {
                    if(meth.IsStatic)
                        Console.WriteLine($"\tPublic Static {meth.ReturnParameter.Name} {meth}");
                    else Console.WriteLine($"\tPublic {meth.ReturnParameter.Name} {meth}");
                }
                else
                {
                    if (meth.IsStatic)
                        Console.WriteLine($"\tPrivate Static {meth.ReturnParameter.Name} {meth}");
                    else Console.WriteLine($"\tPrivate {meth.ReturnParameter.Name} {meth}");
                }
            }

            Console.WriteLine("\nCreating instance via Activator.CreateInstance:");
            var a = Activator.CreateInstance(myType);
            var prop1 = myType.GetProperty("Name");
            prop1.SetValue(a, "Valeriy");
            var prop2 = myType.GetProperty("Id");
            prop2.SetValue(a, 2);
            Console.WriteLine($"Name: {prop1.GetValue(a)} \tId: {prop2.GetValue(a)}");

            Console.WriteLine("\nCalling methods via reflexia: ");
            var meth1 = myType.GetMethod("PublicMethod");
            meth1.Invoke(a, null);

            Console.WriteLine("\nCalling private method via reflexia");
            foreach(var meth in myType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                //Console.Write(meth.Name == "PrivateMethod" ? meth.Invoke(a, null) : "");
                //Одно из этих двух, и то, и то работает
                if (!meth.IsPublic && meth.Name == "PrivateMethod")
                    Console.WriteLine(meth.Invoke(a, null));
            }
            Console.WriteLine("\n");


        }
        class MyClass
        {
            private int Prop1 { get; set; }
            public string Name { get; set; }
            public int Id { get; set; }
            public MyClass() { }
            public MyClass(int prop1, int prop2)
            {
                Prop1 = prop1;
                Id = prop2;
            }
            private MyClass(int prop1)
            {
                Prop1 = prop1;
            }   

            public void PublicMethod()
            {
                Console.WriteLine("Hello World!");
            }
            private string PrivateMethod()
            {
                return "Im private method! Tssss";
            }
        }
    }
}
