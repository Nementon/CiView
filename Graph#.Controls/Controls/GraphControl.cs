using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using GraphSharp.Controls.Animations;
using System.Windows.Media;

namespace GraphSharp.Controls
{
    [TemplatePart(Name = GraphControl.GraphAreaName, Type = typeof(GraphCanvas))]
    //[ContentProperty("Graph")]
    public class GraphControl : Control
    {
        private const string GraphAreaName = "GraphArea";

        private GraphCanvas GraphArea { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // Unhook events from former template parts
            if (null != GraphArea) GraphArea.Children.Clear();

            // Access new template parts
            GraphArea = GetTemplateChild(GraphAreaName) as GraphCanvas;

            if (GraphArea != null)
            {
                // Add controls to GraphArea
            }

        }
    }
}
