using System.Collections.Generic;
using NUnit.Framework;

namespace Robot
{
    [TestFixture]
    public class Robot_Tests : TestBase
    {
        [Test]
        public void RunTests()
        {
            RunTest(ReadWriteTest, "ReadWrite testing");
            RunTest(ReadWritePopTest, "ReadWritePop testing");
            RunTest(ReadWriteCopyTest, "ReadWriteCopy testing");
            RunTest(PushWriteConcatTest, "PushWriteConcat testing");
            RunTest(JmpLabelPushWritePopPushTest, "JmpLabelPushWritePopPush testing");
            RunTest(JmpLabelPushReplaceoneCopyPopSwapWriteTest, "JmpLabelPushReplaceoneCopyPopSwapWrite testing");
        }

        public void JmpLabelPushWritePopPushTest()
        {

            var commands = new List<string>
            {
                "JMP main",
                "LABEL 123",
                "PUSH '456'",
                "WRITE",
                "POP",
                "JMP",
                "LABEL main",
                "PUSH 'point'",
                "JMP 123",
                "LABEL point",
                "PUSH '111'",
                "WRITE"
            };
            var input = new List<string>();
            var expectedOutput = new List<string> { "456", "111" };

            var output = Robot.Evaluate(commands, input);

            OutputShouldBe(output, expectedOutput);
        }

        public void JmpLabelPushReplaceoneCopyPopSwapWriteTest()
        {
            var commands = new List<string>
            {
                "JMP main",
                "",
                "LABEL contains.l",
                "PUSH 'contains.l1'",
                "COPY 3",
                "COPY 1",
                "COPY 4",
                "REPLACEONE",
                "POP",
                "POP",
                "POP",
                "POP",
                "JMP",
                "",
                "LABEL contains.l1",
                "POP",
                "POP",
                "SWAP 1 2",
                "POP",
                "JMP",
                "",
                "LABEL main",
                "PUSH 'contains.true'",
                "PUSH 'contains.false'",
                "PUSH '123'",
                "PUSH '1123'",

                "LABEL contains.true",
                "PUSH 'true'",
                "WRITE",
                "JMP end",

                "LABEL contains.false",
                "PUSH 'false'",
                "WRITE",
                "JMP end",

                "LABEL end"
            };
            var input = new List<string>();
            var expectedOutput = new List<string> { "true"};

            var output = Robot.Evaluate(commands, input);

            OutputShouldBe(output, expectedOutput);
        }

        public void ReadWriteTest()
        {
            var commands = new List<string> {
                "READ",
                "WRITE"
            };
            var lines = new List<string> {"123"};

            var output = Robot.Evaluate(commands, lines);

            OutputShouldBe(lines, output);
        }

        public void ReadWriteCopyTest()
        {
            var commands = new List<string>
            {
                "READ",
                "COPY 1",
                "WRITE"
            };
            var lines = new List<string> {"123"};
            var output = Robot.Evaluate(commands, lines);

            OutputShouldBe(lines, output);
        }

        public void ReadWritePopTest()
        {
            var commands = new List<string>
            {
                "READ",
                "READ",
                "POP",
                "WRITE"
            };
            var input = new List<string> { "123", "456" };
            var expectedOutput = new List<string> { "123" };

            var output = Robot.Evaluate(commands, input);

            OutputShouldBe(expectedOutput, output);
        }

        public void PushWriteConcatTest()
        {
            var commands = new List<string>
            {
                "PUSH 'b'",
                "PUSH 'a'",
                "CONCAT",
                "WRITE" 
                
            };
            var input = new List<string>();
            var expectedOutput = new List<string> { "ab" };

            var output = Robot.Evaluate(commands, input);

            OutputShouldBe(output, expectedOutput);
        }
    }
}