using Newtonsoft.Json;
using System;

namespace AnimalGuesser
{
    public class Question
    {
        public Question PositiveResponse { get; set; }
        public Question NegativeResponse { get; set; }
        public string QuestionText { get; set; }
        public string Result { get; set; }

        public Question(Question positiv, Question negative, string questionText, string result)
        {
            QuestionText = questionText;
            Result = result;
            PositiveResponse = positiv;
            NegativeResponse = negative;
        }

        public void ExecuteQuestion()
        {
            Console.WriteLine(QuestionText);
            Console.WriteLine("1 - да\n0 - нет");
            if (Console.ReadLine() == "1")
            {
                if (PositiveResponse == null)
                    Console.WriteLine("Вы загадали " + Result);
                else
                    PositiveResponse.ExecuteQuestion();
            }
            else if(NegativeResponse == null)
                ExtendTree();
            else
                NegativeResponse.ExecuteQuestion();
        }

        public void ExtendTree()
        {
            Console.WriteLine("Сдаюсь. Что было загадано?");
            string guesedObject = Console.ReadLine();

            Console.WriteLine("Что отличает загаданый оъект?");
            string objectCharacteristic = Console.ReadLine() ?? "";

            var newNode = new Question(null, null, "Это "+ guesedObject+"?", guesedObject);
            NegativeResponse = new Question(newNode, null, objectCharacteristic + "?", "");
        }
    }

    static class Program
    {
        static void Main()
        {
            const string fileName = "QuestionBase.json";
            var currentNode = JsonConvert.DeserializeObject<Question>(File.ReadAllText(fileName));
            currentNode.ExecuteQuestion();

            string jsonString = JsonConvert.SerializeObject(currentNode);
            File.WriteAllText(fileName, jsonString);
            Console.WriteLine(File.ReadAllText(fileName));
        }
    }
}