using AvalonAssets.Rojy.Graph;
using NUnit.Framework;

namespace AvalonAssets.RojyTests.Graph
{
    [TestFixture]
    public class GraphConnectionTests
    {
        [Test]
        public void Test()
        {
            var connection = new GraphConnection<int>();
            connection.AddPredecessor(1);
            Assert.True(connection.IsPredecessors(1));
            Assert.True(connection.RemovePredecessor(1));
            Assert.False(connection.IsPredecessors(1));
            connection.AddSuccessor(1);
            Assert.True(connection.IsSuccessors(1));
            Assert.True(connection.RemoveSuccessor(1));
            Assert.False(connection.IsSuccessors(1));
            connection.AddPredecessor(1);
            connection.AddSuccessor(1);
            Assert.True(connection.IsPredecessors(1));
            Assert.True(connection.IsSuccessors(1));
            Assert.True(connection.RemovePredecessor(1));
            Assert.False(connection.IsPredecessors(1));
            connection.AddPredecessor(1);
            Assert.True(connection.RemoveSuccessor(1));
            Assert.False(connection.IsSuccessors(1));
            Assert.True(connection.RemovePredecessor(1));
            Assert.False(connection.RemovePredecessor(1));
            Assert.False(connection.RemoveSuccessor(1));
        }
    }
}