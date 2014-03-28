using UnityEngine;
using System.Collections;

/*
    Author: Jon Wiedmann
    Email: jonwiedmann@gmail.com
    Repo: https://github.com/jonmann20/EZGui
    License: MIT

    EZGui is a class for easily laying out text with Unity's OnGUI() function.  It is well suited for text call-to-actions, and simple notification animations.
    
    0) Add this file anywhere in the Assets folder (does NOT need to be attached to a game object in the heirarchy)
    1) Setup a target resolution, and layout your GUI relative to this, using FULLW instead of Screen.width etc...
    2) Make a call to init() at the start of your OnGUI() function.


    TODO: btnBackground
*/

public struct EZOpt {   
    public Color? color, hoverColor, activeColor, dropShadow;
    public bool bold, italic, leftJustify;
    public int dropShadowX, dropShadowY;

    public EZOpt(Color color){
        this.color = color;

        this.hoverColor = this.activeColor = this.dropShadow = null;
        this.bold = this.italic = this.leftJustify = false;
        this.dropShadowX = this.dropShadowY = 5;
    }

    public EZOpt(Color color, Color dropShadow) : this(color){
        this.dropShadow = dropShadow;
    }

    public EZOpt(Color color, Color hoverColor, Color activeColor, Color dropShadow) : this(color, dropShadow) {
        this.hoverColor = hoverColor;
        this.activeColor = activeColor;
    }
};

public class EZGUI : MonoBehaviour {

    public const float FULLW = 1920;
    public const float FULLH = 1080;

    public const float HALFW = FULLW / 2;
    public const float HALFH = FULLH / 2;

    struct GUIObject {
        public GUIContent cnt;
        public GUIStyle style;
        public Vector2 size;
        public Rect rect;
    }

    /// <summary>
    /// Scales GUI.matrix relative to FULLW and FULLH.  
    /// Must be called at the start of OnGUI()!
    /// </summary>
    public static void init(){
        float rx = Screen.width / FULLW;
        float ry = Screen.height / FULLH;

        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(rx, ry, 1));
    }

    static GUIObject getGUIObject(string str, int fontSize, float x, float y, EZOpt e){
        GUIObject gObj = new GUIObject();

        gObj.cnt = new GUIContent(str);

        gObj.style = new GUIStyle();
        gObj.style.fontSize = fontSize;
        gObj.style.normal.textColor = e.color ?? Color.white;

        gObj.size = gObj.style.CalcSize(gObj.cnt);
        gObj.rect = new Rect(x - gObj.size.x/2, y - gObj.size.y, gObj.size.x, gObj.size.y);

        if(e.leftJustify){
            gObj.rect.x += gObj.rect.width/2;
        }

        if(e.italic && e.bold) {
            gObj.style.fontStyle = FontStyle.BoldAndItalic;
        }
        else if(e.italic) {
            gObj.style.fontStyle = FontStyle.Italic;
        }
        else if(e.bold) {
            gObj.style.fontStyle = FontStyle.Bold;
        }

        return gObj;
    }

    static void addDropShadow(GUIObject g, EZOpt e) {
        if(e.dropShadow != null && g.style.normal.textColor.a != 0){
            Color prevColor = g.style.normal.textColor;

            Color ds = (Color)e.dropShadow;
            ds.a = prevColor.a;
            g.style.normal.textColor = ds;

            GUI.Label(new Rect(g.rect.x + e.dropShadowX, g.rect.y + e.dropShadowY, g.rect.width, g.rect.height), g.cnt, g.style);

            g.style.normal.textColor = prevColor;
        }
    }

    static bool checkMouse(GUIObject g, Color? hoverColor, Color? activeColor){
        if(hoverColor != null) {
            Vector2 mousePos = GUIUtility.ScreenToGUIPoint(Input.mousePosition);
            mousePos.y = FULLH - mousePos.y;

            if(g.rect.Contains(mousePos)){
                if(activeColor != null && Input.GetMouseButton(0)) {
                    g.style.normal.textColor = (Color)activeColor;
                }
                else {
                    g.style.normal.textColor = (Color)hoverColor;
                }

                return true;
            }
        }

        return false;
    }

    #region GUI.Label

    /// <summary>
    /// Draws str with center at (x, y).
    /// </summary>
    public static void placeTxt(string str, int fontSize, float x, float y, EZOpt? opt=null){
        EZOpt e = opt ?? new EZOpt();
        GUIObject g = getGUIObject(str, fontSize, x, y, e);
        addDropShadow(g, e);
        
        GUI.Label(g.rect, g.cnt, g.style);
    }

    /// <summary>
    /// Str will wrap to fit in width.  NOTE: text is left justified and top aligned (this includes placement)
    /// </summary>
    public static void wrapTxt(string str, int fontSize, float x, float y, float width, EZOpt? opt=null) {
        EZOpt e = opt ?? new EZOpt();
        GUIObject g = getGUIObject(str, fontSize, x, y, e);

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = g.style.fontSize;
        style.normal.textColor = g.style.normal.textColor;


        // drop shadow
        if(e.dropShadow != null) {
            GUIStyle drpStyle = new GUIStyle(style);
            drpStyle.normal.textColor = (Color)e.dropShadow;

            GUI.Label(new Rect(x + e.dropShadowX, y + e.dropShadowY, width, FULLH), str, drpStyle);
        }

        GUI.Label(new Rect(x, y, width, FULLH), str, style);
    }

    /// <summary>
    /// Fades str in and out.
    /// </summary>
    public static void blinkTxt(string str, int fontSize, float x, float y, EZOpt? opt=null){
        EZOpt e = opt ?? new EZOpt();
        GUIObject g = getGUIObject(str, fontSize, x, y, e);

        Color c = g.style.normal.textColor;
        c.a = Mathf.PingPong(Time.time, 1);
        g.style.normal.textColor = c;

        addDropShadow(g, e);

        GUI.Label(g.rect, g.cnt, g.style);
    }

    /// <summary>
    /// Turns str on and off.
    /// </summary>
    public static void flashTxt(string str, int fontSize, float x, float y, EZOpt? opt=null) {
        EZOpt e = opt ?? new EZOpt();
        GUIObject g = getGUIObject(str, fontSize, x, y, e);
		
        if(Time.time % 2 < 1) {
            addDropShadow(g, e);

            GUI.Label(g.rect, g.cnt, g.style);
        }
	}

    /// <summary>
    /// Scales str's fontSize [0, 9].
    /// </summary>
    public static void pulseTxt(string str, int fontSize, float x, float y, EZOpt? opt=null) {
        EZOpt e = opt ?? new EZOpt();

        float pp = Mathf.PingPong(Time.time, 0.9f);
		pp *= 10;
		fontSize += (int)pp;

        GUIObject g = getGUIObject(str, fontSize, x, y, e);

        addDropShadow(g, e);

		GUI.Label(g.rect, g.cnt, g.style);
	}

    #endregion GUI.Label

    #region GUI.Button

    /// <summary>
    /// Draws str with center at (x, y).
    /// </summary>
    /// <returns>True if button was clicked.</returns>
    public static bool placeBtn(string str, int fontSize, float x, float y, EZOpt? opt=null) {
        EZOpt e = opt ?? new EZOpt();
        GUIObject g = getGUIObject(str, fontSize, x, y, e);

        checkMouse(g, e.hoverColor, e.activeColor);
        addDropShadow(g, e);
        
        return GUI.Button(g.rect, g.cnt, g.style);
    }

    /// <summary>
    /// Fades str in and out.
    /// </summary>
    /// <returns>True if button was clicked.</returns>
    public static bool blinkBtn(string str, int fontSize, float x, float y, EZOpt? opt=null) {
        EZOpt e = opt ?? new EZOpt();
        GUIObject g = getGUIObject(str, fontSize, x, y, e);

        Color c = g.style.normal.textColor;
        c.a = Mathf.PingPong(Time.time, 1);
        g.style.normal.textColor = c;

        checkMouse(g, e.hoverColor, e.activeColor);
        addDropShadow(g, e);

        return GUI.Button(g.rect, g.cnt, g.style);
    }

    /// <summary>
    /// Turns str's alpha value on and off.
    /// </summary>
    /// <returns>True if button was clicked.</returns>
    public static bool flashBtn(string str, int fontSize, float x, float y, EZOpt? opt=null) {
        EZOpt e = opt ?? new EZOpt();
        GUIObject g = getGUIObject(str, fontSize, x, y, e);

        Color c = g.style.normal.textColor;
        c.a = (Time.time % 2 < 1) ? 1 : 0;
        g.style.normal.textColor = c;

        checkMouse(g, e.hoverColor, e.activeColor);
        addDropShadow(g, e);

        return GUI.Button(g.rect, g.cnt, g.style);
    }

    /// <summary>
    /// Scales str's fontSize [0, 9].
    /// </summary>
    /// <returns>True if button was clicked.</returns>
    public static bool pulseBtn(string str, int fontSize, float x, float y, EZOpt? opt=null) {
        EZOpt e = opt ?? new EZOpt();

        GUIObject g = getGUIObject(str, fontSize, x, y, e);
        bool isHover = checkMouse(g, e.hoverColor, e.activeColor);

        if(!isHover){
            float pp = Mathf.PingPong(Time.time, 0.9f);
            pp *= 10;
            fontSize += (int)pp;

            g = getGUIObject(str, fontSize, x, y, e);
        }
        else {
            fontSize += 9;  // make text fullsize on hover

            g = getGUIObject(str, fontSize, x, y, e);

            if(e.activeColor != null && Input.GetMouseButton(0)) {
                g.style.normal.textColor = (Color)e.activeColor;
            }
            else {
                g.style.normal.textColor = (Color)e.hoverColor;
            }
        }

        addDropShadow(g, e);

        return GUI.Button(g.rect, g.cnt, g.style);
    }

    #endregion GUI.Button

    #region GUI.Window

    public static bool placeWindow(string str, int fontSize, float x, float y, int height, GUI.WindowFunction cb, Color bgColor, EZOpt? opt=null) {
        EZOpt e = opt ?? new EZOpt();
        GUIObject g = getGUIObject(str, fontSize, x, y, e);

        GUI.backgroundColor = bgColor;
        //GUI.contentColor = e.color ?? Color.white;

        GUIStyle style = new GUIStyle(GUI.skin.window);
        style.fontSize = g.style.fontSize;
        style.normal.textColor = g.style.normal.textColor;

        style.onActive.textColor = style.normal.textColor;
        style.active.textColor = style.normal.textColor;
        style.onActive.background = style.onNormal.background;
        style.active.background = style.onNormal.background;


        GUI.Window(0, new Rect(x, y, height, height), cb, str, style);//, g.style);

        return placeBtn("Close", 25, x + height, y, new EZOpt(Color.white, Color.red, new Color(0.9f, 0, 0), new Color(0.1f, 0.1f, 0.1f)));
    }

    #endregion GUI.Window
}


//style.alignment = TextAnchor.MiddleCenter;
//g.style.wordWrap = true;