using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Xml.Serialization;


namespace StarWars_Homework
{

    class Program
    {
        static void Main(string[] args)
        {
            /*Реализовать консольное приложение, которое взаимодействует с https://swapi.co/ для получения 
             * персонажей саги звёздных войн по одному. Программа:
             * 1. Запрашивает у пользователь некоторое число (идентификатор)
             * 2. По данному идентификатору делает следующее:
                * а. Проверяет есть ли персонаж с указанным идентификатором в файле file.xml 
                     и если есть - десериализует и отображает его
                * б. Если в файле данных нет, то делает запрос на https://swapi.co/ для получения этого персонажа.
                     Сохраняет его в file.xml и отображает пользователю*/


            string file = @"file.xml";
            string json;
            string pathUrl = "https://swapi.co/api/people/";
            var character = new StarWarsCharacter();
            var characterColection = new List<StarWarsCharacter>();
            var newCharacterColection = new List<StarWarsCharacter>();
            var formatter = new XmlSerializer(typeof(List<StarWarsCharacter>));

            //Сериализация Json из URL

            Console.Write("Введите число(идентификатор персонажа) от  1 до 88: ");
            string starWarsCharacter = Console.ReadLine();


            using (WebClient client = new WebClient())
            {
                json = client.DownloadString(pathUrl + starWarsCharacter + "/");
            }

            character = JsonConvert.DeserializeObject<StarWarsCharacter>(json);

            characterColection.Add
                (
                new StarWarsCharacter
                {
                    Name = character.Name,
                    Height = character.Height,
                    Mass = character.Mass,
                    Hair_color = character.Hair_color,
                    Skin_color = character.Skin_color,
                    Eye_color = character.Eye_color,
                    Birth_year = character.Birth_year,
                    Gender = character.Gender,
                    Homeworld = character.Homeworld,
                    Films = character.Films,
                    Species = character.Species,
                    Vehicles = character.Vehicles,
                    Starships = character.Starships,
                    Created = character.Created,
                    Edited = character.Edited,
                    Url = character.Url
                }
                );

            

            if (File.Exists(file))
            {
                using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
                {
                    newCharacterColection = (List<StarWarsCharacter>)formatter.Deserialize(fs);
                }
            }

            else
            {
                using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, characterColection);

                }
                using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
                {
                    newCharacterColection = (List<StarWarsCharacter>)formatter.Deserialize(fs);
                }
            }
              

            foreach (var newCharacter in newCharacterColection)
            {
                if (newCharacter.Url== character.Url)
                {
                    Console.WriteLine("Персонаж ранее уже был сериализован в xml-файл, либо был сериализован впервые!!!");
                    ShowCharacter(newCharacterColection);
                    break;
                }
                else
                {
                    newCharacterColection.Add
                    (
                    new StarWarsCharacter
                    {
                        Name = character.Name,
                        Height = character.Height,
                        Mass = character.Mass,
                        Hair_color = character.Hair_color,
                        Skin_color = character.Skin_color,
                        Eye_color = character.Eye_color,
                        Birth_year = character.Birth_year,
                        Gender = character.Gender,
                        Homeworld = character.Homeworld,
                        Films = character.Films,
                        Species = character.Species,
                        Vehicles = character.Vehicles,
                        Starships = character.Starships,
                        Created = character.Created,
                        Edited = character.Edited,
                        Url = character.Url
                    }
                    );
                    using (FileStream fs = new FileStream(file, FileMode.Create))
                    {
                        
                            formatter.Serialize(fs, newCharacterColection);
                        Console.WriteLine("Новый персонаж успешно добавлен в xml-файл!!!");
                    }

                    using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
                    {
                        newCharacterColection = (List<StarWarsCharacter>)formatter.Deserialize(fs);
                    }
                    ShowCharacter(newCharacterColection);
                    break;
                }
            }
            Console.ReadLine();
            
        }

        static void ShowCharacter(List<StarWarsCharacter> characters)
        {
            foreach (var character in characters)
            {
                Console.WriteLine("*****************************************");
                Console.WriteLine($"Полное имя персонажа {character.Name}\n" +
              $"Параметры:\n" +
              $"  Рост: {character.Height}\n" +
              $"  Вес: {character.Mass}\n" +
              $"  Цвет волос: {character.Hair_color}\n" +
              $"  Цвет кожи: {character.Skin_color}\n" +
              $"  Цвет глаз: {character.Eye_color}\n" +
              $"Год рождения: {character.Birth_year}\n" +
              $"Пол: {character.Gender}\n" +
              $"Родина: {character.Homeworld}\n" +
              $"\nФильмы:");

                int cnt = 1;
                foreach (var film in character.Films)
                {
                    Console.WriteLine($"    {cnt}. {film}");
                    cnt++;
                }
                cnt = 1;

                Console.WriteLine($"\nВид:");
                foreach (var species in character.Species)
                {
                    Console.WriteLine($"{cnt}. {species}");
                    cnt++;
                }
                cnt = 1;

                Console.WriteLine($"\nТранспортное средство:\n");
                foreach (var vehicles in character.Vehicles)
                {
                    Console.WriteLine($"{cnt}. {vehicles}");
                    cnt++;
                }
                cnt = 1;

                Console.WriteLine($"\nЗвездолет:\n");
                foreach (var starship in character.Starships)
                {
                    Console.WriteLine($"{cnt}. {starship}");
                    cnt++;
                }

                Console.WriteLine($"\nСоздан: {character.Created}\n" +
                $"\nОтредактирован: {character.Edited}\n" +
                $"\nURL-aдрес: {character.Url}");
                Console.WriteLine("*****************************************");
            }
        }
    }

}
