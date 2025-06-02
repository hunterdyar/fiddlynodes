using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using fiddlyNodes;

public class RectTransform
{
    public readonly Element Element;

    private Vector2 _localPosition;
    private Vector2 _size;
    private Vector2 _anchor = new Vector2(0, 0); // Anchor point (0,0 = top-left, 0.5,0.5 = center, 1,1 = bottom-right)
    private Vector2 _pivot = new Vector2(0, 0);   // Pivot point for the object itself
    private bool _scaleWithParent = true;        // Whether to scale proportionally when parent scales
    /// <summary>
    /// InputPassthrough makes the object ignore itself and always test children for mouse hovering, contained points, etc.
    /// </summary>
    private bool _inputPassthrough = false;
    //true if child can be interaccted with while visually beyond parent.
    private bool _extendsParent;
    private RectTransform _parent;
    private List<RectTransform> _children;
    
    // Cached world values - recalculated when dirty
    private Vector2 _worldPosition;
    private Rectangle _worldBounds;
    private bool _isDirty = true;
    
    public RectTransform(Element element)
    {
        Element = element;
        _children = new List<RectTransform>();
        _localPosition = Vector2.Zero;
        _size = new Vector2(100, 100);
    }
    
    public RectTransform(Vector2 position, Vector2 size, Element element) : this(element)
    {
        _localPosition = position;
        _size = size;
    }
    
    #region Properties

    public void SetPassthrough(bool value)
    {
        _inputPassthrough = value;
    }
    /// <summary>
    /// Local position relative to parent anchor point
    /// </summary>
    public Vector2 LocalPosition
    {
        get => _localPosition;
        set
        {
            if (_localPosition != value)
            {
                _localPosition = value;
                MarkDirty();
            }
        }
    }
    
    /// <summary>
    /// Local size of this transform
    /// </summary>
    public Vector2 Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                Vector2 oldSize = _size;
                _size = value;
                
                // If size changed and we have children that scale with parent, scale them too
                if (_scaleWithParent && oldSize.X > 0 && oldSize.Y > 0)
                {
                    Vector2 scaleRatio = new Vector2(value.X / oldSize.X, value.Y / oldSize.Y);
                    ScaleChildren(scaleRatio);
                }
                
                MarkDirty();
            }
        }
    }
    
    /// <summary>
    /// Local anchor point within parent (0,0 = top-left of parent, 1,1 = bottom-right of parent)
    /// </summary>
    public Vector2 Anchor
    {
        get => _anchor;
        set
        {
            if (_anchor != value)
            {
                _anchor = value;
                MarkDirty();
            }
        }
    }
    
    /// <summary>
    /// Local pivot point of this object (0,0 = top-left, 0.5,0.5 = center, 1,1 = bottom-right)
    /// </summary>
    public Vector2 Pivot
    {
        get => _pivot;
        set
        {
            if (_pivot != value)
            {
                _pivot = value;
                MarkDirty();
            }
        }
    }
    
    /// <summary>
    /// Whether this transform should scale proportionally when its parent's size changes
    /// </summary>
    public bool ScaleWithParent
    {
        get => _scaleWithParent;
        set
        {
            if (_scaleWithParent != value)
            {
                _scaleWithParent = value;
            }
        }
    }
    
    /// <summary>
    /// World position (calculated)
    /// </summary>
    public Vector2 WorldPosition
    {
        get
        {
            UpdateWorldTransform();
            return _worldPosition;
        }
    }

    /// <summary>
    /// World pivot position (calculated)
    /// </summary>
    public Vector2 WorldPivotPosition
    {
        get
        {
            UpdateWorldTransform();
            return new Vector2(
                _worldPosition.X + _size.X * _pivot.X,
                _worldPosition.Y + _size.Y * _pivot.Y
            );
        }
    }
    
    /// <summary>
    /// World bounds rectangle (calculated)
    /// </summary>
    public Rectangle WorldBounds
    {
        get
        {
            UpdateWorldTransform();
            return _worldBounds;
        }
    }
    
    /// <summary>
    /// Parent transform
    /// </summary>
    public RectTransform Parent
    {
        get => _parent;
        set
        {
            if (_parent != value)
            {
                // Remove from old parent
                _parent?.RemoveChild(this);
                
                // Set new parent
                _parent = value;
                
                // Add to new parent
                _parent?.AddChild(this);
                
                MarkDirty();
            }
        }
    }
    
    /// <summary>
    /// Read-only list of children
    /// </summary>
    public IReadOnlyList<RectTransform> Children => _children.AsReadOnly();
    
    #endregion
    
    #region Hierarchy Management
    
    /// <summary>
    /// Add a child transform
    /// </summary>
    public void AddChild(RectTransform child)
    {
        if (child == null || _children.Contains(child)) return;
        
        // Remove from previous parent first
        if (child._parent != null && child._parent != this)
        {
            child._parent.RemoveChild(child);
        }
        
        _children.Add(child);
        child._parent = this;
        child.MarkDirty();
    }
    
    /// <summary>
    /// Remove a child transform
    /// </summary>
    public bool RemoveChild(RectTransform child)
    {
        if (child == null || !_children.Contains(child)) return false;
        
        _children.Remove(child);
        child._parent = null;
        child.MarkDirty();
        return true;
    }
    
    /// <summary>
    /// Remove this transform from its parent
    /// </summary>
    public void RemoveFromParent()
    {
        Parent = null;
    }
    
    #endregion
    
    #region Transform Calculations
    
    private void ScaleChildren(Vector2 scaleRatio)
    {
        foreach (var child in _children)
        {
            if (child._scaleWithParent)
            {
                // Scale the child's size proportionally
                child.Size = new Vector2(child._size.X * scaleRatio.X, child._size.Y * scaleRatio.Y);
                
                // Scale the child's local position proportionally
                child._localPosition = new Vector2(child._localPosition.X * scaleRatio.X, child._localPosition.Y * scaleRatio.Y);
                
                // Mark child as dirty to recalculate world transform
                child.MarkDirty();
            }
        }
    }
    
    private void MarkDirty()
    {
        _isDirty = true;
        // Mark all children as dirty too
        foreach (var child in _children)
        {
            child.MarkDirty();
        }
    }
    
    private void UpdateWorldTransform()
    {
        if (!_isDirty) return;
        
        if (_parent == null)
        {
            // Root transform - world position is local position
            _worldPosition = _localPosition - new Vector2(_size.X * _pivot.X, _size.Y * _pivot.Y);
            _extendsParent = false;
            _worldBounds = new Rectangle(_worldPosition.X, _worldPosition.Y, _size.X, _size.Y);

        }
        else
        {
            // Child transform - calculate relative to parent
            _parent.UpdateWorldTransform(); // Ensure parent is up to date
            
            Rectangle parentBounds = _parent.WorldBounds;
            
            // Calculate anchor position within parent
            Vector2 anchorPosition = new Vector2(
                parentBounds.X + parentBounds.Width * _anchor.X,
                parentBounds.Y + parentBounds.Height * _anchor.Y
            );
            
            // Apply local position offset and pivot
            _worldPosition = anchorPosition + _localPosition - new Vector2(_size.X * _pivot.X, _size.Y * _pivot.Y);
            _worldBounds = new Rectangle(_worldPosition.X, _worldPosition.Y, _size.X, _size.Y);

            _extendsParent = _worldBounds.Overlaps(parentBounds);
        }
        
        // Update world bounds
        _isDirty = false;
    }
    
    #endregion
    
    #region Utility Methods
    
    /// <summary>
    /// Check if a world point is inside this transform's bounds
    /// </summary>
    public bool ContainsPoint(Vector2 worldPoint)
    {
        Rectangle bounds = WorldBounds;
        return worldPoint.X >= bounds.X && worldPoint.X <= bounds.X + bounds.Width &&
               worldPoint.Y >= bounds.Y && worldPoint.Y <= bounds.Y + bounds.Height;
    }
    
    /// <summary>
    /// Check if this transform completely contains another transform
    /// </summary>
    public bool ContainsTransform(RectTransform other)
    {
        if (other == null) return false;
        
        Rectangle thisBounds = WorldBounds;
        Rectangle otherBounds = other.WorldBounds;
        
        // Check if other's bounds are completely inside this bounds
        return otherBounds.X >= thisBounds.X &&
               otherBounds.Y >= thisBounds.Y &&
               otherBounds.X + otherBounds.Width <= thisBounds.X + thisBounds.Width &&
               otherBounds.Y + otherBounds.Height <= thisBounds.Y + thisBounds.Height;
    }
    
    /// <summary>
    /// Check if this transform overlaps with another transform (including partial overlaps)
    /// </summary>
    public bool OverlapsTransform(RectTransform other)
    {
        if (other == null) return false;
        
        Rectangle thisBounds = WorldBounds;
        Rectangle otherBounds = other.WorldBounds;
        
        // Check if rectangles overlap using standard rectangle intersection test
        return thisBounds.X < otherBounds.X + otherBounds.Width &&
               thisBounds.X + thisBounds.Width > otherBounds.X &&
               thisBounds.Y < otherBounds.Y + otherBounds.Height &&
               thisBounds.Y + thisBounds.Height > otherBounds.Y;
    }
    
    /// <summary>
    /// Convert a world point to local coordinates
    /// </summary>
    public Vector2 WorldToLocal(Vector2 worldPoint)
    {
        Vector2 worldPos = WorldPosition;
        return worldPoint - worldPos;
    }
    
    /// <summary>
    /// Convert a local point to world coordinates
    /// </summary>
    public Vector2 LocalToWorld(Vector2 localPoint)
    {
        Vector2 worldPos = WorldPosition;
        return worldPos + localPoint;
    }
    
    /// <summary>
    /// Get the center point in world coordinates
    /// </summary>
    public Vector2 GetWorldCenter()
    {
        Rectangle bounds = WorldBounds;
        return new Vector2(bounds.X + bounds.Width * 0.5f, bounds.Y + bounds.Height * 0.5f);
    }
    
    /// <summary>
    /// Draw debug outline of this transform (for debugging purposes)
    /// </summary>
    public void DrawDebug(Color color)
    {
        Rectangle bounds = WorldBounds;
        Raylib.DrawRectangleLines((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height, color);
        
        // Draw a small dot at the pivot point
        Vector2 pivotWorld = LocalToWorld(new Vector2(_size.X * _pivot.X, _size.Y * _pivot.Y));
        Raylib.DrawCircle((int)pivotWorld.X, (int)pivotWorld.Y, 2, color);
    }
    
    /// <summary>
    /// Draw debug outline of this transform and all children recursively
    /// </summary>
    public void DrawDebugHierarchy(Color color)
    {
        DrawDebug(color);
        foreach (var child in _children)
        {
            child.DrawDebugHierarchy(color);
        }
    }
    
    #endregion
    
    #region Convenience Methods
    
    /// <summary>
    /// Set position using separate x and y values
    /// </summary>
    public void SetLocalPosition(float x, float y)
    {
        LocalPosition = new Vector2(x, y);
    }
    
    /// <summary>
    /// Set size using separate width and height values
    /// </summary>
    public void SetSize(float width, float height)
    {
        Size = new Vector2(width, height);
    }
    
    /// <summary>
    /// Set anchor using separate x and y values
    /// </summary>
    public void SetAnchor(float x, float y)
    {
        Anchor = new Vector2(x, y);
    }
    
    /// <summary>
    /// Set pivot using separate x and y values
    /// </summary>
    public void SetPivot(float x, float y)
    {
        Pivot = new Vector2(x, y);
    }
    
    /// <summary>
    /// Set anchor to center (0.5, 0.5)
    /// </summary>
    public void SetAnchorCenter()
    {
        Anchor = new Vector2(0.5f, 0.5f);
    }
    
    /// <summary>
    /// Set pivot to center (0.5, 0.5)
    /// </summary>
    public void SetPivotCenter()
    {
        Pivot = new Vector2(0.5f, 0.5f);
    }
    
    /// <summary>
    /// Translate this transform by world coordinates
    /// </summary>
    public void Translate(float worldX, float worldY)
    {
        Translate(new Vector2(worldX, worldY));
    }
    
    /// <summary>
    /// Translate this transform by a world vector
    /// </summary>
    public void Translate(Vector2 worldOffset)
    {
        if (_parent == null)
        {
            // No parent - world translation equals local translation
            LocalPosition += worldOffset;
        }
        else
        {
            // Has parent - need to convert world offset to local space
            // Since we don't have rotation, this is just a direct offset
            LocalPosition += worldOffset;
        }
    }

    /// <summary>
    /// Scale transform around a specific world point (like mouse position for zooming)
    /// </summary>
    public void ScaleAroundPoint(Vector2 worldPoint, float scale)
    {
        // Get the point relative to current world position
        Vector2 worldPos = WorldPosition;
        Vector2 relativePoint = worldPoint - worldPos;

        // Scale the size
        Size = new Vector2(Size.X * scale, Size.Y * scale);

        // Scale the relative point and adjust position so the world point stays put
        Vector2 scaledRelativePoint = new Vector2(relativePoint.X * scale, relativePoint.Y * scale);
        Vector2 newWorldPos = worldPoint - scaledRelativePoint;

        // Convert back to local position
        if (_parent == null)
        {
            LocalPosition = newWorldPos;
        }
        else
        {
            // This is simplified - you'd need proper world-to-local conversion for complex hierarchies
            LocalPosition += newWorldPos - worldPos;
        }
    }

    //todo: i guess this should be in Element...
    public void GetChildElementOrSelfAtPosition(Vector2 pos, ref List<Element> list)
    {
        bool containsPoint = ContainsPoint(pos);
        if (containsPoint || _inputPassthrough)
        {
            if (Element != null && !_inputPassthrough)//
            {
                list.Add(Element);
            }
        }

        
        foreach (RectTransform child in Children)
        {
            if (child._extendsParent || _inputPassthrough)
            {
                child.GetChildElementOrSelfAtPosition(pos, ref list);
            }
            else
            {
                if (containsPoint && child.ContainsPoint(pos))
                {
                    child.GetChildElementOrSelfAtPosition(pos, ref list);
                }
            }
        }

    }
    
    #endregion
}