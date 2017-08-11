using ConsoleApp1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NSubstitute;
using NSubstitute.Exceptions;
using System.Collections.Generic;

namespace ConsoleApp1.Tests
{
    [TestClass()]
    public class CForTestTests
    {
        [TestMethod()]
        public void AddTest()
        {
            CForTest target = new CForTest();
            Random rnd = new Random();
            int firstNumber = rnd.Next(100) + 1;
            int secondNumber = rnd.Next(100) + 1;
            int result = firstNumber + secondNumber;
            int actual = target.Add(firstNumber, secondNumber);
            Assert.AreEqual(actual, result);
            //Assert.Fail();
        }

        [TestMethod()]
        public void greaterTest()
        {
            CForTest target = new CForTest();
            Random rnd = new Random();
            int from = rnd.Next(100) + 1;
            int adder = rnd.Next(10) + 1;
            int cnt = rnd.Next(1000) + 1;
            int result = cnt * adder + from;
            int actual = target.greater(from, adder, cnt);
            Assert.AreEqual(result, actual);
            //Assert.Fail();
        }

        [TestMethod()]
        public void AddTest1()
        {
            CForTest target = new CForTest();
            string result = "Test";
            target.Mode = "Test";
            Assert.AreEqual(result, target.Mode);
            //Assert.Fail();
        }

       

        [TestMethod]
        public void Test_GetStart()
        {            
            ICalculator  calculator = Substitute.For<ICalculator >();
        }

        [TestMethod]
        public void Test_GetStarted_ReturnSpecifiedValue()
        {
            ICalculator calculator = Substitute.For<ICalculator>();
            calculator.Add(1, 2).Returns(3);

            int actual = calculator.Add(1, 2);
            Assert.AreEqual<int>(3, actual);
            
        }

        [TestMethod]
        public void Test_GetStarted_ReceivedSpecificCall()
        {
            ICalculator calculator = Substitute.For<ICalculator>();
            calculator.Add(1, 2);
            calculator.Received().Add(1, 2);
            calculator.DidNotReceive().Add(5,7);
        }


        [TestMethod]
        [ExpectedException(typeof(ReceivedCallsException))]
        public void Test_GetStarted_DidNotReceivedSpecificCall()
        {
            ICalculator calculator = Substitute.For<ICalculator>();
            calculator.Add(5, 7);
            calculator.Received().Add(1, 2);
        }

        [TestMethod]
        public void Test_GetStarted_SetPropertyValue()
        {
            ICalculator calculator = Substitute.For<ICalculator>();

            calculator.Mode.Returns("DEC");
            //calculator.Mode.Returns("TAL");
            Assert.AreEqual<string>("DEC", calculator.Mode);

            calculator.Mode = "HEX";
            Assert.AreEqual<string>("HEX", calculator.Mode);
        }

        [TestMethod]
        public void Test_GetStarted_MatchArguments()
        {
            ICalculator calculator = Substitute.For<ICalculator>();

            calculator.Add(10, -5);

            calculator.Received().Add(10, Arg.Is<int>(x => x<0||x>5));
            calculator.Received().Add(10, Arg.Is<int>(x => x<0));
        }

        [TestMethod]
        public void Test_GetStarted_PassFuncToReturns()
        {
            ICalculator calculator = Substitute.For<ICalculator>();
           var t= calculator
               .Add(Arg.Any<int>(), Arg.Any<int>())
               .Returns(x => (int)x[0] + (int)x[1]);

            int actual = calculator.Add(5, 10);

            Assert.AreEqual(15, actual);
        }

        [TestMethod]
        public void Test_GetStarted_MultipleValues()
        {
            ICalculator calculator = Substitute.For<ICalculator>();
            calculator.Mode.Returns("HEX", "DEC", "BIN");

            Assert.AreEqual<string>("HEX", calculator.Mode);
            Assert.AreEqual<string>("DEC", calculator.Mode);
            Assert.AreEqual<string>("BIN", calculator.Mode);
        }

        [TestMethod]
        public void Test_GetStarted_RaiseEvents()
        {
            ICalculator calculator = Substitute.For<ICalculator>();
            bool eventWasRaised = false;

            calculator.PowerUp += (sender, args) =>
            {
                eventWasRaised = true;
            };

            calculator.PowerUp += Raise.Event();

            Assert.IsTrue(eventWasRaised);
        }

        [TestMethod]
        public void Test_SettingReturnValue_ReturnsValueWithSpecifiedArguments()
        {
            var calculator = Substitute.For<ICalculator>();
            calculator.Add(1, 2).Returns(3);
            
            Assert.AreEqual(calculator.Add(1, 2), 3);
        }

        [TestMethod]
        public void Test_SettingReturnValue_ReturnsDefaultValueWithDifferentArguments()
        {
            var calculator = Substitute.For<ICalculator>();

            // 设置调用返回值为3
            calculator.Add(1, 2).Returns(3);

            Assert.AreEqual(calculator.Add(1, 2), 3);
            Assert.AreEqual(calculator.Add(1, 2), 3);

            // 当使用不同参数调用时,返回值不是3
            Assert.AreNotEqual(calculator.Add(3, 6), 3);
        }

        [TestMethod]
        public void Test_SettingReturnValue_ReturnsValueFromProperty()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Mode.ReturnsForAnyArgs("DEC");
            Assert.AreEqual(calculator.Mode, "DEC");

            calculator.Mode = "DEC";
            Assert.AreEqual(calculator.Mode, "DEC");
        }

        public interface IFoo
        {
            string Bar(int a, string b);
        }

        [TestMethod]
        public void Test_ReturnFromFunction_CallInfo()
        {
            var foo = Substitute.For<IFoo>();
            foo.Bar(0, "").ReturnsForAnyArgs(x => "Hello " + x.Arg<string>());
            Assert.AreEqual("Hello World", foo.Bar(2, "World"));
        }

        [TestMethod]
        public void Test_ReturnFromFunction_GetCallbackWhenever()
        {
            var calculator = Substitute.For<ICalculator>();

            var counter = 0;
            calculator
              .Add(0, 0)
              .ReturnsForAnyArgs(x =>
              {
                  counter++;
                  return 0;
              });

            calculator.Add(7, 3);
            calculator.Add(2, 2);
            calculator.Add(11, -3);
            Assert.AreEqual(counter, 3);
        }

        [TestMethod]
        public void Test_ReturnFromFunction_UseAndDoesAfterReturns()
        {
            var calculator = Substitute.For<ICalculator>();

            var counter = 0;
            calculator
              .Add(0, 0)
              .ReturnsForAnyArgs(x => 0)
              .AndDoes(x => counter++);

            calculator.Add(7, 3);
            calculator.Add(2, 2);
            Assert.AreEqual(counter, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_MultipleReturnValues_UsingCallbacks()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Mode.Returns(x => "DEC", x => "HEX", x => { throw new Exception(); });
            Assert.AreEqual("DEC", calculator.Mode);
            Assert.AreEqual("HEX", calculator.Mode);
            var result = calculator.Mode;
        }

        public interface ICommand
        {
            void Execute();
        }

        public class OnceOffCommandRunner
        {
            ICommand command;
            public OnceOffCommandRunner(ICommand command)
            {
                this.command = command;
            }
            public void Run()
            {
                if (command == null) return;
                command.Execute();
                command = null;
            }
        }

        [TestMethod]
        public void Test_ClearReceivedCalls_ForgetPreviousCalls()
        {
            var command = Substitute.For<ICommand>();
            var runner = new OnceOffCommandRunner(command);

            // 第一次运行
            runner.Run();
            command.Received().Execute();

            // 忘记前面对command的调用
            command.ClearReceivedCalls();

            // 第二次运行
            runner.Run();
            command.DidNotReceive().Execute();
        }

        public interface IEvents
        {
            void RaiseOrderProcessed(int orderId);
        }

        public interface ICart
        {
            int OrderId { get; set; }
        }

        public interface IOrderProcessor
        {
            void ProcessOrder(int orderId, Action<bool> orderProcessed);
        }

        public class OrderPlacedCommand
        {
            IOrderProcessor orderProcessor;
            IEvents events;
            public OrderPlacedCommand(IOrderProcessor orderProcessor, IEvents events)
            {
                this.orderProcessor = orderProcessor;
                this.events = events;
            }
            public void Execute(ICart cart)
            {
                orderProcessor.ProcessOrder(
                    cart.OrderId,
                    wasOk => { if (wasOk) events.RaiseOrderProcessed(cart.OrderId); }
                );
            }
        }

        [TestMethod]
        public void Test_ActionsWithArgumentMatchers_InvokingCallbacks()
        {
            // Arrange
            var cart = Substitute.For<ICart>();
            var events = Substitute.For<IEvents>();
            var processor = Substitute.For<IOrderProcessor>();
            cart.OrderId = 3;
            // 设置 processor 当处理订单ID为3时，调用回调函数，参数为true
            processor.ProcessOrder(3, Arg.Invoke(true));

            // Act
            var command = new OrderPlacedCommand(processor, events);
            command.Execute(cart);

            // Assert
            events.Received().RaiseOrderProcessed(3);
        }

        [TestMethod]
        public void Test_ActionsWithArgumentMatchers_PerformingActionsWithAnyArgs()
        {
            var calculator = Substitute.For<ICalculator>();

            var firstArgsBeingMultiplied = new List<int>();
            calculator.Multiply(Arg.Do<int>(x => firstArgsBeingMultiplied.Add(x)), 10);

            calculator.Multiply(2, 10);
            calculator.Multiply(5, 10);

            // 由于第二个参数不为10，所以不会被 Arg.Do 匹配
            calculator.Multiply(7, 4567);

            CollectionAssert.AreEqual(firstArgsBeingMultiplied, new[] { 2, 5 });
        }

        [TestMethod]
        public void Test_ActionsWithArgumentMatchers_ArgActionsCallSpec()
        {
            var calculator = Substitute.For<ICalculator>();

            var numberOfCallsWhereFirstArgIsLessThan0 = 0;

            // 指定调用参数：
            // 第一个参数小于0
            // 第二个参数可以为任意的int类型值
            // 当此满足此规格时，为计数器加1。
            calculator
              .Multiply(
                Arg.Is<int>(x => x < 0),
                Arg.Do<int>(x => numberOfCallsWhereFirstArgIsLessThan0++)
              ).Returns(123);

            var results = new[] {
                calculator.Multiply(-4, 3),
                calculator.Multiply(-27, 88),
                calculator.Multiply(-7, 8),
                calculator.Multiply(123, 2) // 第一个参数大于0，所以不会被匹配
            };

            Assert.AreEqual(3, numberOfCallsWhereFirstArgIsLessThan0); // 4个调用中有3个匹配上
            CollectionAssert.AreEqual(results, new[] { 123, 123, 123, 0 }); // 最后一个未匹配
        }
    }
}