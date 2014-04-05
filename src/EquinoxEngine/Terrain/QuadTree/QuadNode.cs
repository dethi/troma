using Microsoft.Xna.Framework;

namespace EquinoxEngine.Terrain
{
    public class QuadNode
    {
        #region Fields

        QuadNode _parent;
        QuadTree _parentTree;
        int _positionIndex;

        bool _isActive;
        bool _isSplit;

        int _nodeDepth;
        int _nodeSize;

        bool HasChildren;

        #region Vertices

        public QuadNodeVertex VertexTopLeft;
        public QuadNodeVertex VertexTop;
        public QuadNodeVertex VertexTopRight;
        public QuadNodeVertex VertexLeft;
        public QuadNodeVertex VertexCenter;
        public QuadNodeVertex VertexRight;
        public QuadNodeVertex VertexBottomLeft;
        public QuadNodeVertex VertexBottom;
        public QuadNodeVertex VertexBottomRight;

        #endregion

        #region Children

        public QuadNode ChildTopLeft;
        public QuadNode ChildTopRight;
        public QuadNode ChildBottomLeft;
        public QuadNode ChildBottomRight;

        #endregion

        #region Neighbors

        public QuadNode NeighborTop;
        public QuadNode NeighborRight;
        public QuadNode NeighborBottom;
        public QuadNode NeighborLeft;

        #endregion

        public BoundingBox Bounds;
        public NodeType NodeType;

        public bool IsSplit
        {
            get { return _isSplit; }
        }

        public bool CanSplit
        {
            get { return (_nodeSize >= 2); }
        }

        public QuadNode Parent
        {
            get { return _parent; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            internal set { _isActive = value; }
        }

        public bool IsInView
        {
            get { return Contains(_parentTree.ViewFrustrum); }
        }

        #endregion

        public QuadNode(NodeType nodeType, int nodeSize, int nodeDepth,
            QuadNode parent, QuadTree parentTree, int positionIndex)
        {
            NodeType = nodeType;
            _nodeSize = nodeSize;
            _nodeDepth = nodeDepth;
            _positionIndex = positionIndex;

            _parent = parent;
            _parentTree = parentTree;

            AddVertices();

            Bounds = new BoundingBox(
                _parentTree.Vertices[VertexTopLeft.Index].Position,
                _parentTree.Vertices[VertexBottomRight.Index].Position);
            Bounds.Min.Y = -950f;
            Bounds.Max.Y = 950f;

            if (nodeSize >= 4)
                AddChildren();

            if (_nodeDepth == 1)
            {
                AddNeighbors();

                VertexTopLeft.Activated = true;
                VertexTopRight.Activated = true;
                VertexCenter.Activated = true;
                VertexBottomLeft.Activated = true;
                VertexBottomRight.Activated = true;
            }
        }

        /// <summary>
        /// Add vertices to the QuadNode
        /// </summary>
        private void AddVertices()
        {
            switch (NodeType)
            {
                case NodeType.TopLeft:
                    VertexTopLeft = _parent.VertexTopLeft;
                    VertexTopRight = _parent.VertexTop;
                    VertexBottomLeft = _parent.VertexLeft;
                    VertexBottomRight = _parent.VertexCenter;
                    break;

                case NodeType.TopRight:
                    VertexTopLeft = _parent.VertexTop;
                    VertexTopRight = _parent.VertexTopRight;
                    VertexBottomLeft = _parent.VertexCenter;
                    VertexBottomRight = _parent.VertexRight;
                    break;

                case NodeType.BottomLeft:
                    VertexTopLeft = _parent.VertexLeft;
                    VertexTopRight = _parent.VertexCenter;
                    VertexBottomLeft = _parent.VertexBottomLeft;
                    VertexBottomRight = _parent.VertexBottom;
                    break;

                case NodeType.BottomRight:
                    VertexTopLeft = _parent.VertexCenter;
                    VertexTopRight = _parent.VertexRight;
                    VertexBottomLeft = _parent.VertexBottom;
                    VertexBottomRight = _parent.VertexBottomRight;
                    break;

                default:
                    VertexTopLeft = new QuadNodeVertex
                    {
                        Activated = true,
                        Index = 0
                    };

                    VertexTopRight = new QuadNodeVertex
                    {
                        Activated = true,
                        Index = VertexTopLeft.Index + _nodeSize
                    };

                    VertexBottomLeft = new QuadNodeVertex
                    {
                        Activated = true,
                        Index = (_parentTree.TopNodeSize + 1) * _parentTree.TopNodeSize
                    };

                    VertexBottomRight = new QuadNodeVertex
                    {
                        Activated = true,
                        Index = VertexBottomLeft.Index + _nodeSize
                    };

                    break;
            }

            VertexTop = new QuadNodeVertex
            {
                Activated = false,
                Index = VertexTopLeft.Index + (_nodeSize / 2)
            };

            VertexLeft = new QuadNodeVertex
            {
                Activated = false,
                Index = VertexTopLeft.Index + (_parentTree.TopNodeSize + 1) * (_nodeSize / 2)
            };

            VertexCenter = new QuadNodeVertex
            {
                Activated = false,
                Index = VertexLeft.Index + (_nodeSize / 2)
            };

            VertexRight = new QuadNodeVertex
            {
                Activated = false,
                Index = VertexLeft.Index + _nodeSize
            };

            VertexBottom = new QuadNodeVertex
            {
                Activated = false,
                Index = VertexBottomLeft.Index + (_nodeSize / 2)
            };
        }

        /// <summary>
        /// Add the 4 childs
        /// </summary>
        void AddChildren()
        {
            //Add top left child
            ChildTopLeft = new QuadNode(
                NodeType.TopLeft,
                _nodeSize / 2,
                _nodeDepth + 1,
                this,
                _parentTree,
                VertexTopLeft.Index);

            //Add top right child
            ChildTopRight = new QuadNode(
                NodeType.TopRight,
                _nodeSize / 2,
                _nodeDepth + 1,
                this,
                _parentTree,
                VertexTop.Index);

            //Add bottom left child
            ChildBottomLeft = new QuadNode(
                NodeType.BottomLeft,
                _nodeSize / 2,
                _nodeDepth + 1,
                this,
                _parentTree,
                VertexLeft.Index);

            //Add bottom right child
            ChildBottomRight = new QuadNode(
                NodeType.BottomRight,
                _nodeSize / 2,
                _nodeDepth + 1,
                this,
                _parentTree,
                VertexCenter.Index);

            HasChildren = true;
        }

        /// <summary>
        /// Update reference to neighboring quads
        /// </summary>
        private void AddNeighbors()
        {
            switch (NodeType)
            {
                case NodeType.TopLeft:
                    //Top neighbor
                    if (_parent.NeighborTop != null)
                        NeighborTop = _parent.NeighborTop.ChildBottomLeft;

                    //Right neighbor
                    NeighborRight = _parent.ChildTopRight;
                    //Bottom neighbor
                    NeighborBottom = _parent.ChildBottomLeft;

                    //Left neighbor
                    if (_parent.NeighborLeft != null)
                        NeighborLeft = _parent.NeighborLeft.ChildTopRight;

                    break;

                case NodeType.TopRight:
                    //Top neighbor
                    if (_parent.NeighborTop != null)
                        NeighborTop = _parent.NeighborTop.ChildBottomRight;

                    //Right neighbor
                    if (_parent.NeighborRight != null)
                        NeighborRight = _parent.NeighborRight.ChildTopLeft;

                    //Bottom neighbor
                    NeighborBottom = _parent.ChildBottomRight;

                    //Left neighbor
                    NeighborLeft = _parent.ChildTopLeft;

                    break;

                case NodeType.BottomLeft:
                    //Top neighbor
                    NeighborTop = _parent.ChildTopLeft;

                    //Right neighbor
                    NeighborRight = _parent.ChildBottomRight;

                    //Bottom neighbor
                    if (_parent.NeighborBottom != null)
                        NeighborBottom = _parent.NeighborBottom.ChildTopLeft;

                    //Left neighbor
                    if (_parent.NeighborLeft != null)
                        NeighborLeft = _parent.NeighborLeft.ChildBottomRight;

                    break;

                case NodeType.BottomRight:
                    //Top neighbor
                    NeighborTop = _parent.ChildTopRight;

                    //Right neighbor
                    if (_parent.NeighborRight != null)
                        NeighborRight = _parent.NeighborRight.ChildBottomLeft;

                    //Bottom neighbor
                    if (_parent.NeighborBottom != null)
                        NeighborBottom = _parent.NeighborBottom.ChildTopRight;

                    //Left neighbor
                    NeighborLeft = _parent.ChildBottomLeft;

                    break;
            }

            if (this.HasChildren)
            {
                ChildTopLeft.AddNeighbors();
                ChildTopRight.AddNeighbors();
                ChildBottomLeft.AddNeighbors();
                ChildBottomRight.AddNeighbors();
            }
        }

        internal void SetActiveVertices()
        {
            if (!_parentTree.Cull || IsInView)
            {
                if (_isSplit && this.HasChildren)
                {
                    ChildTopLeft.SetActiveVertices();
                    ChildTopRight.SetActiveVertices();
                    ChildBottomLeft.SetActiveVertices();
                    ChildBottomRight.SetActiveVertices();
                }
                else
                {
                    #region Top Triangle(s)

                    _parentTree.UpdateBuffer(VertexCenter.Index);
                    _parentTree.UpdateBuffer(VertexTopLeft.Index);

                    if (VertexTop.Activated)
                    {
                        _parentTree.UpdateBuffer(VertexTop.Index);

                        _parentTree.UpdateBuffer(VertexCenter.Index);
                        _parentTree.UpdateBuffer(VertexTop.Index);
                    }
                    _parentTree.UpdateBuffer(VertexTopRight.Index);

                    #endregion

                    #region Right Triangle(s)

                    _parentTree.UpdateBuffer(VertexCenter.Index);
                    _parentTree.UpdateBuffer(VertexTopRight.Index);

                    if (VertexRight.Activated)
                    {
                        _parentTree.UpdateBuffer(VertexRight.Index);

                        _parentTree.UpdateBuffer(VertexCenter.Index);
                        _parentTree.UpdateBuffer(VertexRight.Index);
                    }
                    _parentTree.UpdateBuffer(VertexBottomRight.Index);

                    #endregion

                    #region Bottom Triangle(s)

                    _parentTree.UpdateBuffer(VertexCenter.Index);
                    _parentTree.UpdateBuffer(VertexBottomRight.Index);

                    if (VertexBottom.Activated)
                    {
                        _parentTree.UpdateBuffer(VertexBottom.Index);

                        _parentTree.UpdateBuffer(VertexCenter.Index);
                        _parentTree.UpdateBuffer(VertexBottom.Index);
                    }
                    _parentTree.UpdateBuffer(VertexBottomLeft.Index);

                    #endregion

                    #region Left Triangle(s)

                    _parentTree.UpdateBuffer(VertexCenter.Index);
                    _parentTree.UpdateBuffer(VertexBottomLeft.Index);

                    if (VertexLeft.Activated)
                    {
                        _parentTree.UpdateBuffer(VertexLeft.Index);

                        _parentTree.UpdateBuffer(VertexCenter.Index);
                        _parentTree.UpdateBuffer(VertexLeft.Index);
                    }
                    _parentTree.UpdateBuffer(VertexTopLeft.Index);

                    #endregion
                }
            }
        }

        internal void Activate()
        {
            VertexTopLeft.Activated = true;
            VertexTopRight.Activated = true;
            VertexCenter.Activated = true;
            VertexBottomLeft.Activated = true;
            VertexBottomRight.Activated = true;

            _isActive = true;
        }

        public void EnforceMinimumDepth()
        {
            if (_nodeDepth < _parentTree.MinimumDepth)
            {
                if (HasChildren)
                {
                    _isActive = false;
                    _isSplit = true;

                    ChildTopLeft.EnforceMinimumDepth();
                    ChildTopRight.EnforceMinimumDepth();
                    ChildBottomLeft.EnforceMinimumDepth();
                    ChildBottomRight.EnforceMinimumDepth();
                }
                else
                {
                    Activate();
                    _isSplit = false;
                }
            }
            else if (_nodeDepth == _parentTree.MinimumDepth ||
                (_nodeDepth < _parentTree.MinimumDepth && !HasChildren))
            {
                Activate();
                _isSplit = false;
            }
        }

        /// <summary>
        /// Check if the QuadNode contains the point
        /// </summary>
        public bool Contains(Vector3 point)
        {
            point.Y = 0;
            return (Bounds.Contains(point) == ContainmentType.Contains);
        }

        public bool Contains(BoundingFrustum boundingFrustrum)
        {
            return Bounds.Intersects(boundingFrustrum);
        }

        /// <summary>
        /// Recursive search of the deepest node that contains the point
        /// </summary>
        public QuadNode DeepestNodeWithPoint(Vector3 point)
        {
            if (!Contains(point))
                return null;
            else if (HasChildren)
            {
                if (ChildTopLeft.Contains(point))
                    return ChildTopLeft.DeepestNodeWithPoint(point);
                else if (ChildTopRight.Contains(point))
                    return ChildTopRight.DeepestNodeWithPoint(point);
                else if (ChildBottomLeft.Contains(point))
                    return ChildBottomLeft.DeepestNodeWithPoint(point);
                else
                    return ChildBottomRight.DeepestNodeWithPoint(point);
            }
            else
                return this;
        }

        /// <summary>
        /// Split the node by activating vertices
        /// </summary>
        public void Split()
        {
            if (!_parentTree.Cull || IsInView)
            {
                // Make sure parent node is split
                if (_parent != null && !_parent.IsSplit)
                    _parent.Split();

                if (CanSplit)
                {
                    if (HasChildren)
                    {
                        ChildTopLeft.Activate();
                        ChildTopRight.Activate();
                        ChildBottomLeft.Activate();
                        ChildBottomRight.Activate();

                        _isActive = false;
                    }
                    else
                        _isActive = true;

                    _isSplit = true;
                    VertexTop.Activated = true;
                    VertexLeft.Activated = true;
                    VertexRight.Activated = true;
                    VertexBottom.Activated = true;
                }

                // Make sure neighbor parents are split
                EnsureNeighborParentSplit(NeighborTop);
                EnsureNeighborParentSplit(NeighborRight);
                EnsureNeighborParentSplit(NeighborBottom);
                EnsureNeighborParentSplit(NeighborLeft);

                // Make sure neighbor vertices are active
                if (NeighborTop != null)
                    NeighborTop.VertexBottom.Activated = true;

                if (NeighborRight != null)
                    NeighborRight.VertexLeft.Activated = true;

                if (NeighborBottom != null)
                    NeighborBottom.VertexTop.Activated = true;

                if (NeighborLeft != null)
                    NeighborLeft.VertexRight.Activated = true;
            }
        }

        /// <summary>
        /// Make sure neighbor parents are split
        /// </summary>
        private static void EnsureNeighborParentSplit(QuadNode neighbor)
        {
            if (neighbor != null && neighbor.Parent != null)
            {
                if (!neighbor.Parent.IsSplit)
                    neighbor.Parent.Split();
            }
        }

        /// <summary>
        /// Merge split quad nodes
        /// </summary>
        public void Merge()
        {
            VertexTop.Activated = false;
            VertexLeft.Activated = false;
            VertexRight.Activated = false;
            VertexBottom.Activated = false;

            if (NodeType != NodeType.FullNode)
            {
                VertexTopLeft.Activated = false;
                VertexTopRight.Activated = false;
                VertexBottomLeft.Activated = false;
                VertexBottomRight.Activated = false;
            }

            _isActive = true;
            _isSplit = false;

            if (HasChildren)
            {
                #region Top left

                if (ChildTopLeft.IsSplit)
                {
                    ChildTopLeft.Merge();
                    ChildTopLeft.IsActive = false;
                }
                else
                {
                    ChildTopLeft.VertexTop.Activated = false;
                    ChildTopLeft.VertexLeft.Activated = false;
                    ChildTopLeft.VertexRight.Activated = false;
                    ChildTopLeft.VertexBottom.Activated = false;
                }

                #endregion

                #region Top right

                if (ChildTopRight.IsSplit)
                {
                    ChildTopRight.Merge();
                    ChildTopRight.IsActive = false;
                }
                else
                {
                    ChildTopRight.VertexTop.Activated = false;
                    ChildTopRight.VertexLeft.Activated = false;
                    ChildTopRight.VertexRight.Activated = false;
                    ChildTopRight.VertexBottom.Activated = false;
                }

                #endregion

                #region Bottom left

                if (ChildBottomLeft.IsSplit)
                {
                    ChildBottomLeft.Merge();
                    ChildBottomLeft.IsActive = false;
                }
                else
                {
                    ChildBottomLeft.VertexTop.Activated = false;
                    ChildBottomLeft.VertexLeft.Activated = false;
                    ChildBottomLeft.VertexRight.Activated = false;
                    ChildBottomLeft.VertexBottom.Activated = false;
                }

                #endregion

                #region Bottom right

                if (ChildBottomRight.IsSplit)
                {
                    ChildBottomRight.Merge();
                    ChildBottomRight.IsActive = false;
                }
                else
                {
                    ChildBottomRight.VertexTop.Activated = false;
                    ChildBottomRight.VertexLeft.Activated = false;
                    ChildBottomRight.VertexRight.Activated = false;
                    ChildBottomRight.VertexBottom.Activated = false;
                }

                #endregion
            }
        }
    }
}
