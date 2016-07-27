namespace MindLink.Recruitment.MyChat.Domain.Test.Sensorship
{
    using Censorship;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Linq;

    [TestClass]
    public class PipelineCensorshipPolicyTest
    {
        private const string REPLACEMENT = "*retracted*";

        Mock<ICensorshipPolicy> _fooPolicy;
        Mock<ICensorshipPolicy> _barPolicy;
        Mock<ICensorshipPolicy> _foobarPolicy;

        [TestInitialize]
        public void Setup()
        {
            _fooPolicy = new Mock<ICensorshipPolicy>();
            _fooPolicy.Setup(p => p.Censor(It.Is<string>(s => s.Contains("foo"))))
                .Returns<string>(s => s.Replace("foo", REPLACEMENT));
            _fooPolicy.Setup(p => p.Censor(It.Is<string>(s => !s.Contains("foo"))))
                .Returns<string>(s => s);

            _barPolicy = new Mock<ICensorshipPolicy>();
            _barPolicy.Setup(p => p.Censor(It.Is<string>(s => s.Contains("bar"))))
                .Returns<string>(s => s.Replace("bar", REPLACEMENT));
            _barPolicy.Setup(p => p.Censor(It.Is<string>(s => !s.Contains("bar"))))
                .Returns<string>(s => s);

            _foobarPolicy = new Mock<ICensorshipPolicy>();
            _foobarPolicy.Setup(p => p.Censor(It.Is<string>(s => s.Contains("foobar"))))
                .Returns<string>(s => s.Replace("foobar", REPLACEMENT));
            _foobarPolicy.Setup(p => p.Censor(It.Is<string>(s => !s.Contains("foobar"))))
                .Returns<string>(s => s);
        }

        [TestMethod]
        public void It_should_return_the_initial_value_when_pipeline_is_empty()
        {
            var policy = new PipelineCensorshipPolicy();
            var input = "foo";
            var censored = policy.Censor(input);
            Assert.AreEqual(input, censored);
        }

        [TestMethod]
        public void It_should_add_policies_into_the_pipeline()
        {
            var policy = new PipelineCensorshipPolicy();

            Assert.IsTrue(!policy.Policies.Any());

            policy.AddPolicy(_fooPolicy.Object)
                .AddPolicy(_barPolicy.Object)
                .AddPolicy(_foobarPolicy.Object);

            Assert.AreEqual(3, policy.Policies.Count());
            Assert.AreEqual(1, policy.Policies.Where(p => p == _fooPolicy.Object).Count());
            Assert.AreEqual(1, policy.Policies.Where(p => p == _barPolicy.Object).Count());
            Assert.AreEqual(1, policy.Policies.Where(p => p == _foobarPolicy.Object).Count());
        }

        [TestMethod]
        public void It_should_thread_the_input_value_through_every_policy_in_the_pipeline_in_order()
        {
            var policy = new PipelineCensorshipPolicy();

            policy
                .AddPolicy(_fooPolicy.Object)
                .AddPolicy(_barPolicy.Object)
                .AddPolicy(_foobarPolicy.Object);

            var input = "foobar foobaz";
            var censored = policy.Censor(input);

            _fooPolicy.Verify(p => p.Censor("foobar foobaz"), Times.Once);
            _barPolicy.Verify(p => p.Censor($"{REPLACEMENT}bar {REPLACEMENT}baz"), Times.Once);
            _foobarPolicy.Verify(p => p.Censor($"{REPLACEMENT}{REPLACEMENT} {REPLACEMENT}baz"), Times.Once);

            Assert.AreEqual($"{REPLACEMENT}{REPLACEMENT} {REPLACEMENT}baz", censored);
        }
    }
}
