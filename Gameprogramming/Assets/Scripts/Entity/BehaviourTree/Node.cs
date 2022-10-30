using System.Collections.Generic;

namespace Entity.BehaviourTree
{
    /// <summary>
    /// all possible Nodestates
    /// </summary>
    public enum NodeState
    {
        Running = 0,
        Success = 1,
        Failure = 2
    }

    /// <summary>
    /// Node class, which provides all needed information and behaviour.
    /// </summary>
    public abstract class Node
    {
        protected NodeState _nodeState;

        protected Node _parent;
        protected List<Node> _children = new List<Node>();

        //information from nodes can be added an accessed from other nodes
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public Node()
        {
            _parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (var child in children)
            {
                AttachChildren(child);
            }
        }

        /// <summary>
        /// adds node as a childrean
        /// </summary>
        /// <param name="node">child node</param>
        protected void AttachChildren(Node node)
        {
            node.Parent = this;
            _children.Add(node);
        }

        /// <summary>
        /// sets processed data in the dictionary
        /// </summary>
        /// <param name="key">key of dictionary</param>
        /// <param name="value">value of dictionary</param>
        public void SetData(string key, object value)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext[key] = value;
                return;
            }
            _dataContext.Add(key, value);
        }

        /// <summary>
        /// gets data or searches tree for first appearance of the right key
        /// </summary>
        /// <param name="key"> key you want to get the value from </param>
        /// <returns> value of the key </returns>
        public object GetData(string key)
        {
            object value;
            if (_dataContext.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = _parent;
            while (node != null)
            {
                value = _parent.GetData(key);
                if (value != null)
                {
                    return value;
                }

                node = node._parent;
            }

            return null;
        }

        /// <summary>
        /// takes a key and deletes the entry in the dictionary. If its not part in this dictionary it goes to the parent nodes and searches in their dictionary.
        /// </summary>
        /// <param name="key"> the key you want to delete the entry of the dictionary </param>
        /// <returns> true if the entry was deleted, false if the entry wasn't found </returns>
        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = _parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                {
                    return true;
                }

                node = node._parent;
            }

            return false;
        }

        /// <summary>
        /// Methode to evaluate the nodestate (calculation of each node).
        /// </summary>
        /// <returns> the evaluated nodestate. </returns>
        public virtual NodeState Evaluate() => NodeState.Failure;
    }
}
