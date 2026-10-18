#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// Generates the 5 scenes automatically the first time the project opens.
// Manual fallback: menu  Computer Lab > Build All Scenes
public static class LabBuilder
{
    const string Dir = "Assets/Scenes";
    static readonly string[] Names = { "MainMenu", "ComputerLab", "ServerRoom", "ControlRoom", "Finish" };
    static Material floor, wall, metal, dark, blue, yellow, green, red, desk;

    [InitializeOnLoadMethod]
    static void AutoBuild()
    {
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling) return;
            if (File.Exists(Dir + "/MainMenu.unity")) return;
            try { BuildAll(false); }
            catch (System.Exception e) { Debug.LogError("Computer Lab build failed: " + e); }
        };
    }

    [MenuItem("Computer Lab/Build All Scenes")]
    static void BuildAllMenu() { BuildAll(true); }

    static void BuildAll(bool dialog)
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        if (!AssetDatabase.IsValidFolder(Dir)) AssetDatabase.CreateFolder("Assets", "Scenes");
        MakeMaterials();

        BuildMenu();
        BuildLevel("ComputerLab", 1, "Computer Lab", "Boot all workstations", 4, "ServerRoom");
        BuildLevel("ServerRoom", 2, "Server Room", "Reboot all servers", 3, "ControlRoom");
        BuildLevel("ControlRoom", 3, "Control Room", "Collect the data drives", 3, "Finish");
        BuildFinish();

        EditorBuildSettingsScene[] list = new EditorBuildSettingsScene[Names.Length];
        for (int i = 0; i < Names.Length; i++) list[i] = new EditorBuildSettingsScene(Dir + "/" + Names[i] + ".unity", true);
        EditorBuildSettings.scenes = list;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorSceneManager.OpenScene(Dir + "/MainMenu.unity");
        if (dialog) EditorUtility.DisplayDialog("Computer Lab", "5 scenes created. Press Play.", "OK");
        Debug.Log("Computer Lab: 5 scenes built. Press Play in MainMenu.");
    }

    // ---------- scenes ----------
    static void BuildMenu()
    {
        Scene sc = NewScene();
        Atmosphere(new Color(.4f, .45f, .55f));
        MakeCamera(new Vector3(0, 1.8f, -8), Quaternion.Euler(4, 0, 0), new Color(.03f, .05f, .09f));
        Transform root = new GameObject("Stage").transform;
        Cube(root, "Floor", new Vector3(0, -.25f, 0), new Vector3(40, .5f, 40), floor);
        AddLight("Key Light", new Vector3(3, 3.5f, -3), new Color(.3f, .7f, 1f), 6, 14);

        GameObject pivot = new GameObject("Spinning Monitor");
        pivot.transform.position = new Vector3(4f, 1.8f, 0);
        pivot.AddComponent<Spinner>().degreesPerSecond = new Vector3(0, 35, 0);
        Vector3 p = pivot.transform.position;
        Cube(pivot.transform, "Body", p, new Vector3(2.4f, 1.5f, .15f), dark);
        Cube(pivot.transform, "Screen", p + new Vector3(0, 0, -.09f), new Vector3(2.1f, 1.25f, .05f), blue);
        Cube(pivot.transform, "Stand", p + new Vector3(0, -1f, 0), new Vector3(.25f, .6f, .25f), metal);
        Cube(pivot.transform, "Base", p + new Vector3(0, -1.35f, 0), new Vector3(1.1f, .1f, .8f), metal);

        for (int i = 0; i < 6; i++)
        {
            float a = i * Mathf.PI * 2f / 6f;
            GameObject c = Cube(root, "Data Cube " + i, new Vector3(4f + Mathf.Cos(a) * 3f, 1.8f + Mathf.Sin(a * 2f) * .5f, Mathf.Sin(a) * 3f), Vector3.one * .4f, i % 2 == 0 ? yellow : green);
            c.AddComponent<Spinner>().degreesPerSecond = new Vector3(50, 70, 0);
        }
        new GameObject("Menu UI").AddComponent<MenuUI>();
        Save(sc, "MainMenu");
    }

    static void BuildFinish()
    {
        Scene sc = NewScene();
        Atmosphere(new Color(.4f, .5f, .45f));
        MakeCamera(new Vector3(0, 1.8f, -8), Quaternion.Euler(4, 0, 0), new Color(.02f, .06f, .05f));
        Transform root = new GameObject("Stage").transform;
        Cube(root, "Floor", new Vector3(0, -.25f, 0), new Vector3(40, .5f, 40), floor);
        AddLight("Glow", new Vector3(4, 3.5f, -3), new Color(.3f, 1f, .5f), 6, 14);
        GameObject trophy = Cube(root, "Trophy", new Vector3(4f, 1.8f, 0), new Vector3(1.2f, 1.2f, 1.2f), yellow);
        trophy.AddComponent<Spinner>().degreesPerSecond = new Vector3(30, 60, 20);
        for (int i = 0; i < 8; i++)
        {
            float a = i * Mathf.PI * 2f / 8f;
            GameObject c = Cube(root, "Spark " + i, new Vector3(4f + Mathf.Cos(a) * 2.5f, 1.8f, Mathf.Sin(a) * 2.5f), Vector3.one * .3f, green);
            c.AddComponent<Spinner>().degreesPerSecond = new Vector3(90, 90, 0);
        }
        new GameObject("End UI").AddComponent<EndUI>();
        Save(sc, "Finish");
    }

    static void BuildLevel(string name, int number, string title, string objective, int required, string next)
    {
        Scene sc = NewScene();
        Atmosphere(new Color(.4f, .45f, .52f));
        Transform root = new GameObject("Room").transform;
        Cube(root, "Floor", new Vector3(0, -.25f, 0), new Vector3(16, .5f, 16), floor);
        Cube(root, "Back Wall", new Vector3(0, 2, 8), new Vector3(16, 4, .5f), wall);
        Cube(root, "Front Wall", new Vector3(0, 2, -8), new Vector3(16, 4, .5f), wall);
        Cube(root, "Left Wall", new Vector3(-8, 2, 0), new Vector3(.5f, 4, 16), wall);
        Cube(root, "Right Wall", new Vector3(8, 2, 0), new Vector3(.5f, 4, 16), wall);
        Cube(root, "Light Strip A", new Vector3(-4, 3.9f, 0), new Vector3(.4f, .08f, 8), yellow);
        Cube(root, "Light Strip B", new Vector3(4, 3.9f, 0), new Vector3(.4f, .08f, 8), yellow);
        AddLight("Room Light A", new Vector3(-4, 3.4f, -2), new Color(.8f, .9f, 1f), 6, 12);
        AddLight("Room Light B", new Vector3(4, 3.4f, 3), new Color(.8f, .9f, 1f), 6, 12);

        // exit portal + sliding door
        Cube(root, "Exit Portal", new Vector3(0, 1.5f, 7.68f), new Vector3(3f, 3f, .06f), green);
        GameObject door = Cube(root, "Exit Door", new Vector3(0, 1.5f, 7.45f), new Vector3(3.2f, 3f, .25f), red);
        door.AddComponent<DoorPortal>();
        Cube(root, "Exit Sign", new Vector3(0, 3.5f, 7.5f), new Vector3(2f, .3f, .1f), yellow);

        if (name == "ComputerLab")
        {
            float[] xs = { -5.5f, -2f, 2f, 5.5f };
            for (int i = 0; i < xs.Length; i++) Workstation(root, new Vector3(xs[i], 0, 0), new Color(.1f, 1f, .45f));
            Cube(root, "Whiteboard", new Vector3(-7.65f, 2f, 3.5f), new Vector3(.1f, 1.6f, 3.5f), metal);
        }
        else if (name == "ServerRoom")
        {
            float[] xs = { -5, 0, 5 };
            for (int i = 0; i < xs.Length; i++) Rack(root, new Vector3(xs[i], 0, 3), new Color(.2f, .8f, 1f));
            Cube(root, "Cable Tray", new Vector3(0, 3.5f, 3), new Vector3(14, .12f, .5f), metal);
        }
        else
        {
            Cube(root, "Main Console", new Vector3(0, .5f, 2.2f), new Vector3(4, 1, 1.2f), desk);
            Cube(root, "Wall Display", new Vector3(-7.65f, 2.2f, 0), new Vector3(.1f, 2f, 4f), blue);
            Drive(root, new Vector3(-5, 0, -1));
            Drive(root, new Vector3(5, 0, 1));
            Drive(root, new Vector3(0, 0, 5.2f));
        }

        // player
        GameObject player = new GameObject("Player");
        player.transform.position = new Vector3(0, .1f, -6.5f);
        CharacterController cc = player.AddComponent<CharacterController>();
        cc.height = 1.8f; cc.radius = .35f; cc.center = new Vector3(0, .9f, 0);
        FPSPlayer fp = player.AddComponent<FPSPlayer>();
        GameObject camObj = new GameObject("Player Camera");
        camObj.tag = "MainCamera";
        camObj.transform.SetParent(player.transform);
        camObj.transform.localPosition = new Vector3(0, 1.55f, 0);
        Camera cam = camObj.AddComponent<Camera>();
        cam.fieldOfView = 75; cam.nearClipPlane = .05f;
        cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = new Color(.03f, .05f, .09f);
        camObj.AddComponent<AudioListener>();
        fp.cameraPivot = camObj.transform; fp.cam = cam;

        LevelManager lm = new GameObject("Level Manager").AddComponent<LevelManager>();
        lm.levelTitle = title; lm.objective = objective; lm.required = required;
        lm.levelNumber = number; lm.levelTotal = 3; lm.nextScene = next;
        Save(sc, name);
    }

    // ---------- props ----------
    static void Workstation(Transform root, Vector3 p, Color on)
    {
        Cube(root, "Desk", p + new Vector3(0, .4f, 0), new Vector3(2.4f, .8f, 1.2f), desk);
        GameObject dev = new GameObject("Workstation PC");
        dev.transform.SetParent(root);
        dev.transform.position = p + new Vector3(0, .8f, 0);
        Cube(dev.transform, "Stand", p + new Vector3(0, .95f, 0), new Vector3(.15f, .3f, .15f), metal);
        Cube(dev.transform, "Monitor Body", p + new Vector3(0, 1.3f, 0), new Vector3(1.2f, .75f, .08f), dark);
        GameObject scr = Cube(dev.transform, "Screen", p + new Vector3(0, 1.3f, -.05f), new Vector3(1.05f, .6f, .04f), dark);
        Cube(dev.transform, "Keyboard", p + new Vector3(0, .83f, -.35f), new Vector3(.8f, .04f, .25f), metal);
        Device d = dev.AddComponent<Device>();
        d.screen = scr.GetComponent<Renderer>(); d.onColor = on; d.prompt = "Press E to boot workstation";
    }

    static void Rack(Transform root, Vector3 p, Color on)
    {
        GameObject dev = new GameObject("Server Rack");
        dev.transform.SetParent(root);
        dev.transform.position = p;
        Cube(dev.transform, "Rack Body", p + new Vector3(0, 1.3f, 0), new Vector3(1.2f, 2.6f, .9f), metal);
        GameObject led = Cube(dev.transform, "Status Panel", p + new Vector3(0, 1.9f, -.47f), new Vector3(.95f, .9f, .04f), dark);
        Cube(dev.transform, "Vent", p + new Vector3(0, .8f, -.47f), new Vector3(.95f, .9f, .04f), dark);
        Device d = dev.AddComponent<Device>();
        d.screen = led.GetComponent<Renderer>(); d.onColor = on; d.prompt = "Press E to reboot server";
    }

    static void Drive(Transform root, Vector3 p)
    {
        Cube(root, "Pedestal", p + new Vector3(0, .4f, 0), new Vector3(.7f, .8f, .7f), metal);
        GameObject d = Cube(root, "Data Drive", p + new Vector3(0, 1.05f, 0), new Vector3(.4f, .12f, .6f), blue);
        d.AddComponent<Pickup>().prompt = "Press E to collect data drive";
    }

    // ---------- helpers ----------
    static Scene NewScene() { return EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single); }
    static void Save(Scene s, string name) { EditorSceneManager.SaveScene(s, Dir + "/" + name + ".unity"); }

    static void Atmosphere(Color ambient)
    {
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = ambient;
        GameObject sun = new GameObject("Directional Light");
        Light l = sun.AddComponent<Light>();
        l.type = LightType.Directional; l.intensity = .8f;
        sun.transform.rotation = Quaternion.Euler(50, -30, 0);
    }

    static void MakeCamera(Vector3 pos, Quaternion rot, Color bg)
    {
        GameObject o = new GameObject("Main Camera");
        o.tag = "MainCamera";
        o.transform.position = pos; o.transform.rotation = rot;
        Camera c = o.AddComponent<Camera>();
        c.clearFlags = CameraClearFlags.SolidColor; c.backgroundColor = bg;
        o.AddComponent<AudioListener>();
    }

    static void AddLight(string name, Vector3 pos, Color col, float intensity, float range)
    {
        GameObject o = new GameObject(name);
        o.transform.position = pos;
        Light l = o.AddComponent<Light>();
        l.type = LightType.Point; l.color = col; l.intensity = intensity; l.range = range;
    }

    static GameObject Cube(Transform parent, string name, Vector3 pos, Vector3 scale, Material mat)
    {
        GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
        o.name = name; o.transform.position = pos; o.transform.localScale = scale;
        if (parent != null) o.transform.SetParent(parent, true);
        o.GetComponent<Renderer>().sharedMaterial = mat;
        return o;
    }

    static void MakeMaterials()
    {
        floor = Mat("Floor", new Color(.12f, .16f, .2f));
        wall = Mat("Wall", new Color(.2f, .25f, .31f));
        metal = Mat("Metal", new Color(.3f, .36f, .42f));
        dark = Mat("ScreenOff", new Color(.03f, .05f, .07f));
        blue = Mat("Blue", new Color(.08f, .55f, 1f));
        yellow = Mat("Yellow", new Color(1f, .75f, .15f));
        green = Mat("Green", new Color(.1f, .85f, .4f));
        red = Mat("Red", new Color(.6f, .15f, .12f));
        desk = Mat("Desk", new Color(.35f, .26f, .18f));
    }

    static Material Mat(string name, Color color)
    {
        if (!AssetDatabase.IsValidFolder("Assets/Materials")) AssetDatabase.CreateFolder("Assets", "Materials");
        string path = "Assets/Materials/" + name + ".mat";
        Material m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (m == null)
        {
            Shader sh = GraphicsSettings.defaultRenderPipeline != null ? Shader.Find("Universal Render Pipeline/Lit") : Shader.Find("Standard");
            if (sh == null) sh = Shader.Find("Universal Render Pipeline/Lit");
            if (sh == null) sh = Shader.Find("Standard");
            if (sh == null) sh = Shader.Find("Sprites/Default");
            m = new Material(sh);
            AssetDatabase.CreateAsset(m, path);
        }
        if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", color);
        if (m.HasProperty("_Color")) m.SetColor("_Color", color);
        EditorUtility.SetDirty(m);
        return m;
    }
}
#endif
