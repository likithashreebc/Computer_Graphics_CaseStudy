#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class MetroSafeSceneBuilder
{
    [MenuItem("CS4104/Build MetroSafe Professional Scene")]
    public static void Build()
    {
        if (!EditorUtility.DisplayDialog("MetroSafe VR",
            "Build the enhanced station scene? Save/backup your current scene first.", "Build Scene", "Cancel")) return;

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        Material floor = Mat("Graphite floor", new Color(.13f,.18f,.22f), .18f, .65f);
        Material wall = Mat("Warm concrete", new Color(.48f,.53f,.56f), .05f, .72f);
        Material dark = Mat("Anodized charcoal", new Color(.035f,.055f,.07f), .5f, .35f);
        Material steel = Mat("Brushed steel", new Color(.34f,.42f,.47f), .72f, .28f);
        Material cyan = Glow("Wayfinding cyan", new Color(.03f,.65f,1f), 1.8f);
        Material green = Glow("Emergency green", new Color(.05f,1f,.22f), 1.8f);
        Material amber = Glow("Hazard amber", new Color(1f,.28f,.025f), 1.1f);
        Material white = Mat("Signage white", new Color(.95f,.98f,1f), .05f, .4f);
        Material red = Glow("Alarm red", new Color(1f,.025f,.02f), 1.5f);
        Material[] shirts = {
            Mat("Uniform navy",new Color(.06f,.16f,.32f),.02f,.7f),
            Mat("Clothing teal",new Color(.02f,.38f,.35f),.02f,.7f),
            Mat("Clothing sand",new Color(.66f,.45f,.27f),.02f,.7f),
            Mat("Clothing burgundy",new Color(.4f,.06f,.14f),.02f,.7f),
            Mat("Clothing blue",new Color(.08f,.3f,.7f),.02f,.7f)
        };
        Material skin = Mat("Skin",new Color(.72f,.51f,.37f),0,.8f);

        Cube("Station floor",new Vector3(0,-.25f,0),new Vector3(30,.5f,22),floor);
        Cube("North wall",new Vector3(0,2,11),new Vector3(30,4,.45f),wall);
        Cube("South wall",new Vector3(0,2,-11),new Vector3(30,4,.45f),wall);
        Cube("West wall",new Vector3(-15,2,0),new Vector3(.45f,4,22),wall);
        Cube("East wall",new Vector3(15,2,0),new Vector3(.45f,4,22),wall);
        Cube("North dark dado",new Vector3(0,.55f,10.72f),new Vector3(29.5f,.45f,.08f),dark);
        Cube("South dark dado",new Vector3(0,.55f,-10.72f),new Vector3(29.5f,.45f,.08f),dark);

        // Wall panel rhythm and ceiling light architecture
        for(int x=-13;x<=13;x+=2)
        {
            VisualCube("Wall panel seam",new Vector3(x,2.1f,10.73f),new Vector3(.035f,2.5f,.025f),steel);
            VisualCube("Wall panel seam",new Vector3(x,2.1f,-10.73f),new Vector3(.035f,2.5f,.025f),steel);
        }
        Cube("Ceiling canopy",new Vector3(0,4.25f,0),new Vector3(29.2f,.25f,21.2f),dark);
        for(int z=-9;z<=9;z+=3)
        {
            Cube("Ceiling rib",new Vector3(0,4.08f,z),new Vector3(29,.12f,.16f),steel);
            VisualCube("LED strip",new Vector3(0,3.91f,z+.45f),new Vector3(25,.035f,.12f),cyan);
        }

        // Floor grid and clear evacuation lines
        for(int x=-14;x<15;x+=2) for(int z=-10;z<11;z+=2)
            VisualCube("Tile seam",new Vector3(x+.98f,.012f,z+.98f),new Vector3(.025f,.012f,1.96f),dark);
        VisualCube("Primary route east-west",new Vector3(0,.025f,0),new Vector3(25,.025f,.11f),cyan);
        VisualCube("Primary route north-south",new Vector3(0,.028f,0),new Vector3(.11f,.025f,17),cyan);
        for(int x=-10;x<=10;x+=5) Arrow("Floor route arrow",new Vector3(x,.06f,-.38f),90,cyan);

        // Columns with metal footings and luminous collars
        for(int x=-10;x<=10;x+=5) for(int z=-7;z<=7;z+=7)
        {
            if(Mathf.Abs(x)<2 && Mathf.Abs(z)<3) continue;
            Cube("Column plinth",new Vector3(x,.16f,z),new Vector3(.95f,.32f,.95f),dark);
            Cube("Structural column",new Vector3(x,2,z),new Vector3(.68f,3.45f,.68f),steel);
            Cube("Column LED collar",new Vector3(x,3.55f,z),new Vector3(.72f,.09f,.72f),cyan);
            Cube("Column safety band",new Vector3(x,.42f,z),new Vector3(.72f,.06f,.72f),amber);
        }

        MetroSafeManager manager = new GameObject("SIMULATION | Crowd Manager").AddComponent<MetroSafeManager>();
        CreateExit(manager,"WEST EXIT",new Vector3(-13.45f,.08f,0),green,180);
        CreateExit(manager,"EAST EXIT",new Vector3(13.45f,.08f,0),green,0);
        CreateExit(manager,"NORTH EXIT",new Vector3(0,.08f,9.45f),green,90);
        CreateExit(manager,"SOUTH EXIT",new Vector3(0,.08f,-9.45f),green,-90);
        Sign("EXIT  ←  WEST",new Vector3(-11.8f,3.25f,0),new Vector3(0,90,0),dark,green,3.8f,.85f);
        Sign("EAST  →  EXIT",new Vector3(11.8f,3.25f,0),new Vector3(0,-90,0),dark,green,3.8f,.85f);
        Sign("NORTH EXIT  ↑",new Vector3(0,3.25f,8.25f),Vector3.zero,dark,green,3.8f,.85f);
        Sign("SOUTH EXIT  ↓",new Vector3(0,3.25f,-8.25f),new Vector3(0,180,0),dark,green,3.8f,.85f);

        // Incident/hazard area
        VisualCube("Hazard perimeter",new Vector3(0,.035f,0),new Vector3(5.8f,.04f,3.6f),amber);
        VisualCube("Hazard inner pad",new Vector3(0,.065f,0),new Vector3(5.45f,.04f,3.25f),dark);
        GameObject barrier=Cube("Incident barrier [B]",new Vector3(0,.88f,0),new Vector3(3.4f,1.65f,.48f),amber);
        manager.incidentBarrier=barrier.transform; manager.incidentRenderer=barrier.GetComponent<Renderer>();
        for(int x=-2;x<=2;x++) Cube("Barrier reflective stripe",new Vector3(x*.62f,1,-.251f),new Vector3(.28f,.85f,.025f),white);
        Cylinder("Alarm beacon",new Vector3(0,2,0),.18f,.22f,red);
        Light beacon=new GameObject("Alarm beacon light").AddComponent<Light>();
        beacon.type=LightType.Point;beacon.color=new Color(1,.1f,.03f);beacon.intensity=2;beacon.range=7;beacon.transform.position=new Vector3(0,2.3f,0);

        // Platform benches and information kiosk
        for(int z=-6;z<=6;z+=6)
        {
            Cube("Platform bench seat",new Vector3(-12.2f,.55f,z),new Vector3(2.2f,.16f,1.1f),dark);
            Cube("Platform bench base",new Vector3(-12.2f,.28f,z),new Vector3(1.8f,.4f,.75f),steel);
        }
        Cube("Information kiosk base",new Vector3(7,.55f,6.8f),new Vector3(1.2f,1.1f,.65f),dark);
        Cube("Information kiosk display",new Vector3(7,1.3f,6.8f),new Vector3(1.05f,.42f,.08f),cyan);

        // 100 stylized pedestrian models; simple geometry keeps the prototype self-contained.
        Random.InitState(4104);
        int created=0,attempts=0;
        while(created<100 && attempts<2500)
        {
            attempts++;
            Vector3 p=new Vector3(Random.Range(-10.5f,10.5f),.06f,Random.Range(-7.5f,7.5f));
            if(Mathf.Abs(p.x)<2.7f && Mathf.Abs(p.z)<1.9f) continue;
            GameObject person=new GameObject("Pedestrian "+(created+1).ToString("000"));
            person.transform.position=p;
            CharacterController cc=person.AddComponent<CharacterController>();
            cc.height=1.55f;cc.radius=.22f;cc.center=new Vector3(0,.78f,0);cc.stepOffset=.2f;
            Material shirt=shirts[Random.Range(0,shirts.Length)];
            Part(person,"Torso",PrimitiveType.Capsule,new Vector3(0,.92f,0),new Vector3(.34f,.43f,.23f),shirt);
            Part(person,"Head",PrimitiveType.Sphere,new Vector3(0,1.47f,0),new Vector3(.23f,.25f,.23f),skin);
            Part(person,"Left leg",PrimitiveType.Capsule,new Vector3(-.105f,.35f,0),new Vector3(.105f,.27f,.12f),dark);
            Part(person,"Right leg",PrimitiveType.Capsule,new Vector3(.105f,.35f,0),new Vector3(.105f,.27f,.12f),dark);
            Part(person,"Left arm",PrimitiveType.Capsule,new Vector3(-.235f,.91f,0),new Vector3(.09f,.28f,.09f),shirt);
            Part(person,"Right arm",PrimitiveType.Capsule,new Vector3(.235f,.91f,0),new Vector3(.09f,.28f,.09f),shirt);
            MetroSafeAgent agent=person.AddComponent<MetroSafeAgent>();
            agent.manager=manager;agent.agentId=created+1;agent.walkSpeed=Random.Range(1.1f,1.65f);
            manager.agents.Add(agent);created++;
        }

        GameObject lightRoot=new GameObject("Lighting | station key");
        Light key=lightRoot.AddComponent<Light>();key.type=LightType.Directional;key.intensity=1;
        lightRoot.transform.rotation=Quaternion.Euler(48,-28,0);
        RenderSettings.ambientMode=UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight=new Color(.4f,.46f,.52f);
        RenderSettings.fog=true;RenderSettings.fogColor=new Color(.2f,.25f,.3f);RenderSettings.fogMode=FogMode.ExponentialSquared;RenderSettings.fogDensity=.004f;

        GameObject target=new GameObject("Camera Focus");target.transform.position=Vector3.zero;
        GameObject cameraObject=new GameObject("Main Camera");cameraObject.tag="MainCamera";
        Camera cam=cameraObject.AddComponent<Camera>();cam.fieldOfView=48;cam.nearClipPlane=.1f;cam.farClipPlane=180;
        MetroSafeOrbitCamera orbit=cameraObject.AddComponent<MetroSafeOrbitCamera>();orbit.target=target.transform;
        cameraObject.transform.position=new Vector3(0,22,-29);cameraObject.transform.LookAt(Vector3.zero);
        new GameObject("UI | Evacuation Dashboard").AddComponent<MetroSafeDashboard>().manager=manager;

        Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(scene,"Assets/Scenes/MetroSafe_Professional.unity");
        EditorUtility.DisplayDialog("MetroSafe Scene Built",
            "Professional stylized station created with 100 pedestrians, architecture, exit signage, routes, incident barrier and dashboard. Open Assets/Scenes/MetroSafe_Professional.unity and press Play.", "OK");
    }

    static Material Mat(string n,Color c,float metal,float smooth)
    {
        Shader s=Shader.Find("Universal Render Pipeline/Lit");if(s==null)s=Shader.Find("Standard");
        Material m=new Material(s);m.name=n;m.color=c;
        if(m.HasProperty("_Metallic"))m.SetFloat("_Metallic",metal);
        if(m.HasProperty("_Smoothness"))m.SetFloat("_Smoothness",smooth);
        return m;
    }
    static Material Glow(string n,Color c,float power)
    {
        Material m=Mat(n,c,.05f,.35f);
        if(m.HasProperty("_EmissionColor")){m.EnableKeyword("_EMISSION");m.SetColor("_EmissionColor",c*power);}
        return m;
    }
    static GameObject Cube(string n,Vector3 p,Vector3 s,Material m)
    {
        GameObject o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name=n;o.transform.position=p;o.transform.localScale=s;o.GetComponent<Renderer>().sharedMaterial=m;return o;
    }
    static GameObject VisualCube(string n,Vector3 p,Vector3 s,Material m)
    {
        GameObject o=Cube(n,p,s,m);Object.DestroyImmediate(o.GetComponent<Collider>());return o;
    }
    static void Arrow(string n,Vector3 p,float yaw,Material m)
    {
        GameObject a=VisualCube(n,p,new Vector3(.42f,.035f,.09f),m);a.transform.rotation=Quaternion.Euler(0,yaw,0);
        GameObject tip=VisualCube(n+" head",p+Quaternion.Euler(0,yaw,0)*new Vector3(.2f,0,0),new Vector3(.16f,.035f,.25f),m);tip.transform.rotation=Quaternion.Euler(0,yaw,0);
    }
    static void Cylinder(string n,Vector3 p,float r,float h,Material m)
    {
        GameObject o=GameObject.CreatePrimitive(PrimitiveType.Cylinder);o.name=n;o.transform.position=p;o.transform.localScale=new Vector3(r*2,h,r*2);o.GetComponent<Renderer>().sharedMaterial=m;
    }
    static void Part(GameObject parent,string n,PrimitiveType type,Vector3 lp,Vector3 scale,Material m)
    {
        GameObject o=GameObject.CreatePrimitive(type);o.name=n;o.transform.SetParent(parent.transform,false);o.transform.localPosition=lp;o.transform.localScale=scale;o.GetComponent<Renderer>().sharedMaterial=m;
        Collider c=o.GetComponent<Collider>();if(c!=null)Object.DestroyImmediate(c);
    }
    static void Sign(string label,Vector3 pos,Vector3 rot,Material back,Material glow,float w,float h)
    {
        GameObject root=new GameObject("Wayfinding | "+label);root.transform.position=pos;root.transform.rotation=Quaternion.Euler(rot);
        GameObject board=Cube("Sign backing",Vector3.zero,new Vector3(w,h,.12f),back);board.transform.SetParent(root.transform,false);
        GameObject strip=Cube("Illuminated sign header",new Vector3(0,h*.42f,-.07f),new Vector3(w,.045f,.025f),glow);strip.transform.SetParent(root.transform,false);
        GameObject t=new GameObject("Sign text");t.transform.SetParent(root.transform,false);t.transform.localPosition=new Vector3(0,0,-.075f);
        TextMesh tm=t.AddComponent<TextMesh>();tm.text=label;tm.fontSize=72;tm.characterSize=.018f;tm.anchor=TextAnchor.MiddleCenter;tm.alignment=TextAlignment.Center;tm.color=Color.white;tm.fontStyle=FontStyle.Bold;
    }
    static void CreateExit(MetroSafeManager manager,string label,Vector3 p,Material mat,float yaw)
    {
        GameObject root=new GameObject(label);root.transform.position=p;root.transform.rotation=Quaternion.Euler(0,yaw,0);
        GameObject pad=Cube("Exit floor marker",Vector3.zero,new Vector3(2,.12f,1.4f),mat);pad.transform.SetParent(root.transform,false);
        Cube("Portal left",new Vector3(-.85f,1.35f,0),new Vector3(.12f,2.7f,.16f),mat).transform.SetParent(root.transform,false);
        Cube("Portal right",new Vector3(.85f,1.35f,0),new Vector3(.12f,2.7f,.16f),mat).transform.SetParent(root.transform,false);
        Cube("Portal lintel",new Vector3(0,2.7f,0),new Vector3(1.82f,.12f,.16f),mat).transform.SetParent(root.transform,false);
        MetroSafeExit exit=root.AddComponent<MetroSafeExit>();exit.exitLabel=label;exit.statusRenderer=pad.GetComponent<Renderer>();manager.exits.Add(exit);
    }
}
#endif
