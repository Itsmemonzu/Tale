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
using System.Numerics;
using System.Net.Mime;

namespace Tale
{
    public class ApplicationArguments
    {
        public string? create { get; set; }
        public bool list { get; set; } = false;
        public string? delete { get; set; }
        public string? date { get; set; }
        public bool commands { get; set; } = false;
        public string? view { get; set; } 
        public string? edit { get; set; }
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

            p.Setup(arg => arg.view)
              .As('v', "view");   

            p.Setup(arg => arg.edit) 
              .As('e', "edit"); 

            p.Setup(arg => arg.date)
              .As("date");

            p.Setup(arg => arg.commands)
              .As("cmd");
            
            var result = p.Parse(args);
            
          // Journal

            string? createName = p.Object.create??" ";
            string? deleteName= p.Object.delete??" ";
            bool? list = p.Object.list;

            // The path where StreamWriter/Reader will write or read a .txt file
            string path = Environment.CurrentDirectory+@$"\journals";
            
            // If Directory does not exist.
            if(!Directory.Exists(path))
            {
              Directory.CreateDirectory(path);
            }

          // Help command  
          try
          {
            // If --command == true
            if (p.Object.commands == true)
            {
              Console.WriteLine(" Here's a list of all the commands and a brief explanation on what they do: ");
              Console.WriteLine("");
              Console.WriteLine(" cmd    => Shows the list of all commands");
              Console.WriteLine(" create => Creates a new Journal. Example: tale.cs --create {name}");
              Console.WriteLine(" delete => Deletes a mentioned Journal. Example: tale.cs --delete {name}");
              Console.WriteLine(" date   => Adds date to a Journal if mentioned. Example: tale.cs --create {name} --date {givenDate}");
              Console.WriteLine(" list   => Shows the list of all existing Journals. Example: tale.cs --list");
              Console.WriteLine("");
            }

          }
          catch{

          }
          try
          {
            // Create Journal

            //If --create does not == null
            if (result.HasErrors == false && !createName.Equals(" "))
            {
              Console.WriteLine("");
              Console.WriteLine(" Write your Journal below: ");
              Console.WriteLine("");
              
              var includedParts = new info();
              includedParts.date = p.Object.date;
              includedParts.content = Console.ReadLine();

              // Serializing into Json
              var serializeJournal = Newtonsoft.Json.JsonConvert.SerializeObject(includedParts);
              
              // Writing file
              using (StreamWriter writer = new StreamWriter(path+$@"\{createName}.txt"))
              {
                writer.Write(serializeJournal);
              
                // Return message
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
            // If --list command == true
            if(p.Object.list == true)
            {
              // Read files in directory
              var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
              int amount = files.Count();
              Console.WriteLine("");
              Console.WriteLine($" You have {amount} Journal(s) in your library!");
              Console.WriteLine("");

              // Display files
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
                if(String.IsNullOrEmpty(DeserializedFile?.date) == false)
                {;
                Console.WriteLine(Path.GetFileNameWithoutExtension("  "+file)+ $" [{DeserializedFile.date}]");
                }

                // If date does not exist
                if(String.IsNullOrEmpty(DeserializedFile?.date) == true)
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

          //Delete Journal
            
            // If --deleteName does not == null && File existss
          if (result.HasErrors == false && !deleteName.Equals(" ") && File.Exists(Path.Combine(path, deleteName+".txt")))
            {
              var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
              
              // If exists
              if(File.Exists(Path.Combine(path, deleteName+".txt")))
              {
                File.Delete(Path.Combine(path, deleteName+".txt"));
                Console.WriteLine("");
                Console.WriteLine("The file has been deleted!");
                Console.WriteLine("");
              }
            } 
            // If --deleteName does not == null && File existss
          if(result.HasErrors == false && !deleteName.Equals(" ") && !File.Exists(Path.Combine(path, deleteName+".txt")))
            {
              Console.WriteLine("");
              Console.WriteLine("The file does not exist.");
              Console.WriteLine("");
            } 


          // View Journal
            string? viewJournal = p.Object.view??" ";

            if (!viewJournal.Equals(" "))
            {
              string fileContent;
              using(StreamReader reader = new StreamReader(path+$@"\{viewJournal}.txt"))
              {
                fileContent = reader.ReadToEnd();
              }
              var DeserializedFile = Newtonsoft.Json.JsonConvert.DeserializeObject<info>(fileContent);

              if(String.IsNullOrEmpty(DeserializedFile?.date) == false)
              {
                Console.WriteLine("");
                Console.WriteLine("Name: "+viewJournal+" Date: "+DeserializedFile?.date);
                Console.WriteLine("");
                Console.WriteLine("Content:");
                Console.WriteLine(DeserializedFile?.content);
                Console.WriteLine("");
              }
              if(String.IsNullOrEmpty(DeserializedFile?.date) == true)
              {
                Console.WriteLine("");
                Console.WriteLine("Name: "+viewJournal );
                Console.WriteLine("");
                Console.WriteLine("Content:");
                Console.WriteLine(DeserializedFile?.content);
                Console.WriteLine("");
              } 
            }

          // Edit Journal
            string? editJournal = p.Object.edit??" ";

            if (!editJournal.Equals(" "))
            {
              string fileContent;
              using(StreamReader reader = new StreamReader(path+$@"\{editJournal}.txt"))
              {
                fileContent = reader.ReadToEnd();
              }
              var DeserializedFile = Newtonsoft.Json.JsonConvert.DeserializeObject<info>(fileContent);
              
              Console.WriteLine("");
              Console.WriteLine("Please write the part that you want to edit: ");
              Console.WriteLine("");
              string? subject = Console.ReadLine();

              // If not null
              if(!String.IsNullOrEmpty(subject))
              {
                Console.WriteLine("You want to replace it with:");
                Console.WriteLine("");
                string? replacer = Console.ReadLine();
                
                // If subject exists in DeserializedFile.content
                if(DeserializedFile.content.Contains(subject))
                {
                  // Replace the text
                  string fileRead = File.ReadAllText(path+$@"\{editJournal}.txt");
                  fileRead = fileRead.Replace(subject, replacer);

                  // Write the file
                  File.WriteAllText(path+$@"\{editJournal}.txt", fileRead);

                  Console.WriteLine("Your Journal has been edited succesfully!");
                  Console.WriteLine("");
                }

                // If subject doesn't exist in DeserializedFile.content
                if(!DeserializedFile.content.Contains(subject))
                {
                  Console.WriteLine("The part you wanted to edit does not exist!");
                  Console.WriteLine("");
                }
              }

              // If null
              if(String.IsNullOrEmpty(subject))
              {
                Console.WriteLine("You have entered a null value!");
              }
            } 
          }
        }  
    public class info
    {
      public string? content { get; set; }
      public string? date { get; set; }
    }
}