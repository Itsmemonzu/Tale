﻿/*
MIT License
Copyright (c) 2023 Monzu77

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

/*
        ,----,                             
      ,/   .`|                             
    ,`   .'  :           ,--,              
  ;    ;     /         ,--.'|              
.'___,/    ,'          |  | :              
|    :     |           :  : '              
;    |.';  ;  ,--.--.  |  ' |      ,---.   
`----'  |  | /       \ '  | |     /     \  
    '   :  ;.--.  .-. ||  | :    /    /  | 
    |   |  ' \__\/: . .'  : |__ .    ' / | 
    '   :  | ," .--.; ||  | '.'|'   ;   /| 
    ;   |.' /  /  ,.  |;  :    ;'   |  / | 
    '---'  ;  :   .'   \  ,   / |   :    | 
           |  ,     .-./---`-'   \   \  /  
            `--`---'              `----'  
    A story-writer for your terminal.
*/

using Fclp;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Tale
{
    public class ApplicationArguments
    {
        public string? create { get; set; }
        public bool list { get; set; } = false;
        public string? delete { get; set; }
        public string? date { get; set; }
    }
    public static class MainRenderer
    {
        public static async Task Main(string[] args)
        {
            // CommandLineParser
            var p = new FluentCommandLineParser<ApplicationArguments>();

            p.Setup(arg => arg.create)
              .As('c', "create");

            p.Setup(arg => arg.list)
              .As('l', "list");

            p.Setup(arg => arg.delete)
              .As('d', "delete");      

            p.Setup(arg => arg.date)
              .As("date");
            
            var result = p.Parse(args);
            
            // Journal
            string? createName = p.Object.create??" ";
            string? deleteName= p.Object.delete;
            bool? list = p.Object.list;

              // The path where StreamWriter will write a .txt file
            string path = Environment.CurrentDirectory+@$"\journals";
          
          try
          {
            // Create Journal
            if (result.HasErrors == false && !createName.Equals(" "))
            {
              Console.WriteLine("");
              Console.WriteLine(" Write your Journal below: ");
              Console.WriteLine("");

              var includedParts = new info();
              includedParts.date = p.Object.date;
              includedParts.content = Console.ReadLine();

              var serializeJournal = Newtonsoft.Json.JsonConvert.SerializeObject(includedParts);

              using (StreamWriter writer = new StreamWriter(path+$@"\{createName}.txt"))
              {
                writer.Write(serializeJournal);
              
              Console.WriteLine("");
              Console.WriteLine(" Your Journal has been saved!");
              Console.WriteLine("");
              }
            }
          }catch (Exception e) {
            Console.WriteLine("\n An error has occured! Are you sure that your file name is mentioned properly?\n\n"+e+"\n");
          }

          // Show Journal List
          try
          {
            if(p.Object.list == true)
            {
              var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
              int amount = files.Count();
              Console.WriteLine("");
              Console.WriteLine($" You have {amount} Journal(s) in your library!");
              Console.WriteLine("");
              foreach (string file in files) 
              {
                //Deserializing content for Date
                string fileContent;
                using(StreamReader reader = new StreamReader(file))
                {
                  fileContent = reader.ReadToEnd();
                }
                var DeserializedFile = Newtonsoft.Json.JsonConvert.DeserializeObject<info>(fileContent);
                // If date exists 
                if(!String.IsNullOrEmpty(DeserializedFile?.date))
                {
                Console.WriteLine(Path.GetFileNameWithoutExtension("  "+file+$"[ {DeserializedFile?.date}]"));
                }

                // If date does not exist
                if(String.IsNullOrEmpty(DeserializedFile?.date))
                {
                Console.WriteLine(Path.GetFileNameWithoutExtension("  "+file));
                }
              }      
              Console.WriteLine("");
            }
          }catch (Exception e){
            Console.WriteLine("\n An error has occured! Are you sure you have any Journals in your library?");
            Console.WriteLine("");
            Console.WriteLine(e);
          }
          
        } 
    }
    public class info
    {
      public string? content { get; set; }
      public string? date { get; set; }
    }
}