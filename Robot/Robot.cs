using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Robot
{
    public delegate void RobotAction(string arg);
    public class Robot : IRobot
    {
        public List<string> Evaluate(List<string> commands, IEnumerable<string> input)
        {
            Invoker inv = new Invoker();
            inv.PreProcess(commands);
            var res = inv.Execute(input);
            return res;
        }
        void IRobot.Evaluate(List<string> commands)
        {
            throw new NotImplementedException();
        }
    }

    public class Invoker
    {
        #region "Methods"
        private readonly Dictionary<string, RobotAction> _instruction;

        private void Nope(string arg)
        {
            _point++;
        }

        private void Write(string arg)
        {
            if (stack == null || stack.Count == 0) return;
            results.Add(stack.Peek());
            _point++;
        }

        private void Read(string arg)
        {
            if (Input == null) throw new ArgumentException("Read {0}", "");

            var text = Input.ElementAt(_argspoint);
            stack.Push(text.ToLower());
            _argspoint++;
            _point++;
        }

        private void Pop(string arg)
        {
            stack.Pop();
            _point++;
        }

        private void Jump(string arg)
        {
            if (arg == null)
            {
                _point = labelPointers[stack.Pop()];
            }
            else _point = labelPointers[arg];

        }

        private void ReplaceOne(string arg)
        {
            string[] param = new string[4];
            for (int i = 0; i < param.Length; i++)
            {
                param[i] = stack.Pop();
            }

            if (param[0].Contains(param[1]))
            {
                int indexOf = param[0].IndexOf(param[1]);
                string res1 = param[0].Remove(indexOf, param[1].Count());
                string res2 = res1.Insert(indexOf, param[2]);
                stack.Push(res2);

            }
            else
            {
                stack.Push(param[0]);
                Jump(param[3]);
            }
            _point++;
        }

        private void Concat(string arg)
        {
            string arg1 = stack.Pop();
            string arg2 = stack.Pop();

            var concat = string.Concat(arg1, arg2);
            stack.Push(concat);
            _point++;
        }

        private void Push(string arg)
        {
            string s = arg.Trim(new char[] { '\'' });
            if (s.Contains("''")) s.Replace("''", "'");
            stack.Push(s);
            _point++;
        }

        private void Copy(string arg)
        {
            int index = stack.Count - Int32.Parse(arg);
            stack.Copy(index);
            _point++;
        }

        private void Swap(string arg)
        {
            if (arg == null) throw new ArgumentException("Swap {0}");
            string[] arr = arg.Split(' ');

            int arg1 = Int32.Parse(arr[0]);
            int arg2 = Int32.Parse(arr[1]);
            int length = stack.Count;
            stack.Swap(length - arg1, length - arg2);
            _point++;
        }

        #endregion

        private readonly CustomStack<string> stack;
        private readonly Dictionary<string, int> labelPointers;
        private int _point;
        private int _argspoint;

        public IEnumerable<string> Input { get; set; }



        private List<string> results;

        private List<KeyValuePair<RobotAction, string>> program;

        public Invoker()
        {
            stack = new CustomStack<string>();
            labelPointers = new Dictionary<string, int>();
            _point = 0;
            _argspoint = 0;

            results = new List<string>();

            _instruction = new Dictionary<string, RobotAction>()
            {
                { "JMP", Jump},
                { "READ", Read },
                { "POP", Pop },
                { "WRITE", Write },
                { "CONCAT", Concat },
                { "REPLACEONE", ReplaceOne},
                { "NOPE", Nope },
                { "PUSH", Push },
                { "COPY", Copy },
                { "SWAP", Swap },
            };
        }

        public void PreProcess(List<string> commands)
        {
            if (commands == null || commands.Count == 0)
                throw new ArgumentException("Empty");

            var pairs = new List<KeyValuePair<RobotAction, string>>();
            try
            {
                for (int i = 0; i < commands.Count; i++)
                {
                    string com = commands[i];
                    if (com == null || com == "") com = "NOPE";
                    int spaceIndex = com.IndexOf(' ');
                    string left, right;
                    if (spaceIndex == -1)
                    {
                        left = com;
                        right = null;
                    }
                    else
                    {
                        left = com.Substring(0, spaceIndex);
                        right = com.Substring(spaceIndex + 1);
                        right.ToLower();
                    }
                    if (left == "LABEL")
                    {
                        if (right == null || right.Contains(' '))
                            throw new ArgumentException("Label {0}", right);
                        labelPointers.Add(right, i);
                        left = "NOPE";
                        right = null;
                    }
                    var action = _instruction[left];
                    pairs.Add(new KeyValuePair<RobotAction, string>(action, right));
                }
                program = pairs;
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Key");
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<string> Execute(IEnumerable<string> input)
        {
            try
            {
                Input = input;
                if (program == null) throw new Exception();
                bool isAlive = true;
                int progrCount = program.Count();
                while (isAlive)
                {
                    if (_point >= progrCount) { isAlive = false; continue; }
                    var kvp = program[_point];
                    kvp.Key(kvp.Value);

                }
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }


    public class CustomStack<T>
    {
        private T[] arr;
        private int count;

        public CustomStack()
        {
            count = 0;
            arr = new T[5];
        }

        public T Pop()
        {
            int popIndex = count - 1;
            if (count > 0)
            {
                count--;
                return arr[popIndex];
            }
            else
            {
                return arr[count];
            }

        }

        public void Push(T item)
        {
            if (count == arr.Length)
            {
                Array.Resize(ref arr, arr.Length + 1);
            }
            arr[count] = item;
            count++;
        }

        public void Clear()
        {
            count = 0;
        }

        public T Peek()
        {
            return arr[count - 1];
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        public void Swap(int i, int j)
        {
            int max = arr.Count() - 1;
            if (i < 0 || j < 0 || i > max || j > max)
            {
                throw new ArgumentException();
            }
            T help = arr[i];
            arr[i] = arr[j];
            arr[j] = help;
        }

        public void Copy(int index)
        {
            if (index < 0 || index > arr.Count() - 1)
            {
                throw new ArgumentException();
            }
            T copy = arr[index];
            Push(copy);
        }
    }
}