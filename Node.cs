/*
Dachely Otero Argote            A01377812
José María Aguíñiga Díaz        A01376669 
José Rodrigo Narváez Berlanga   A01377566
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Chimera
{

    class Node : IEnumerable<Node>
    {

        IList<Node> children = new List<Node>();

        public Node this[int index]
        {
            get
            {
                return children[index];
            }
        }

        public int getLength()
        {
            var total = 0;
            foreach (var child in children)
            {
                total += 1;
            }
            return total;
        }

        public Token AnchorToken { get; set; }

        public void Add(Node node)
        {
            children.Add(node);
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return children.GetEnumerator();
        }

        System.Collections.IEnumerator
                System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", GetType().Name, AnchorToken);
        }

        public string ToStringTree()
        {
            var sb = new StringBuilder();
            TreeTraversal(this, "", sb);
            return sb.ToString();
        }

        static void TreeTraversal(Node node, string indent, StringBuilder sb)
        {
            sb.Append(indent);
            sb.Append(node);
            sb.Append('\n');
            foreach (var child in node.children)
            {
                TreeTraversal(child, indent + "  ", sb);
            }
        }

    }
}
