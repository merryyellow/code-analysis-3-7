using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEngine;
using UnityEditor;


public class NewFeatures : MonoBehaviour
{
    [MenuItem("Tools/Test Roslyn Samples/New Language Feature")]
    static void Test()
    {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(
@"using System;
 
namespace TopLevel
{
    public class Program
    {
        RGBColor FromRainbow(Rainbow colorBand) =>
            colorBand switch
            {
                Rainbow.Red    => new RGBColor(0xFF, 0x00, 0x00),
                Rainbow.Orange => new RGBColor(0xFF, 0x7F, 0x00),
                Rainbow.Yellow => new RGBColor(0xFF, 0xFF, 0x00),
                Rainbow.Green  => new RGBColor(0x00, 0xFF, 0x00),
                Rainbow.Blue   => new RGBColor(0x00, 0x00, 0xFF),
                Rainbow.Indigo => new RGBColor(0x4B, 0x00, 0x82),
                Rainbow.Violet => new RGBColor(0x94, 0x00, 0xD3),
                _              => throw new ArgumentException(),
            };
	}

    public enum Rainbow
    {
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Indigo,
        Violet
    }

    class RGBColor
    {
        public RGBColor(int r, int g, int b) { }
    }
}");

        var root = (CompilationUnitSyntax)tree.GetRoot();

        var visitor = new SwitchArmVisitor();
        visitor.Visit(root);
    }
}

class SwitchArmVisitor : CSharpSyntaxWalker
{
    public override void VisitSwitchExpressionArm(SwitchExpressionArmSyntax node)
    {
        base.VisitSwitchExpressionArm(node);

        Debug.Log(node.Pattern.GetText());
    }
    
}
