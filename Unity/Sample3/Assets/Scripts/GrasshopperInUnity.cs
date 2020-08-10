using System.Collections.Generic;
using UnityEngine;
using Rhino;

[ExecuteInEditMode]
public class GrasshopperInUnity : MonoBehaviour
{

  public GameObject geoPrefab;
  private List<GameObject> _gameObjects = new List<GameObject>();
  //private List<Mesh> _meshes = new List<Mesh>();

  public GrasshopperInUnity()
  {
    Rhino.Runtime.HostUtils.RegisterNamedCallback("Unity:FromGrasshopper", FromGrasshopper);
  }

  // This function will be called from a component in Grasshopper
  void FromGrasshopper(object sender, Rhino.Runtime.NamedParametersEventArgs args)
  {
    Rhino.Geometry.GeometryBase[] values;
    if (args.TryGetGeometry("mesh", out values))
    {
      var meshFilters = GetComponentsInChildren<MeshFilter>();

      foreach (var meshFilter in meshFilters)
      {
        if (meshFilter.sharedMesh != null)
        {
          DestroyImmediate(meshFilter.sharedMesh);
        }
      }

      if (values.Length != _gameObjects.Count)
      {
        foreach(var gb in _gameObjects)
        {
          DestroyImmediate(gb);
        }
        foreach (var meshFilter in meshFilters)
        {
          DestroyImmediate(meshFilter.gameObject);
        }
        _gameObjects.Clear();

        for(int i=0; i<values.Length; i++)
        {
          GameObject instance = (GameObject) Instantiate(geoPrefab);
          instance.transform.SetParent(transform);
          _gameObjects.Add(instance);
        }
      }

      //for(int i=0; i<_meshes.Count; i++)
      //{
      //  Destroy(_meshes[i]);
      //}

      //_meshes.Clear();

      for (int i = 0; i < values.Length; i++)
      {
        _gameObjects[i].GetComponent<MeshFilter>().mesh = (values[i] as Rhino.Geometry.Mesh).ToHost();
      }
    }
    //if (_mesh != null)
    //{
    //  gameObject.GetComponent<MeshFilter>().mesh = _mesh.ToHost();
    //}
  }

  //Rhino.Geometry.Mesh _mesh;

  // Start is called before the first frame update
  void Start()
  {
    string script = "!_-Grasshopper _W _S ENTER";
    Rhino.RhinoApp.RunScript(script, false);

  }

  // Update is called once per frame
  void Update()
  {
    //var pt = Camera.main.gameObject.transform.position.ToRhino();
    //using (var args = new Rhino.Runtime.NamedParametersEventArgs())
    //{
    //  args.Set("point", new Rhino.Geometry.Point(pt));
    //  Rhino.Runtime.HostUtils.ExecuteNamedCallback("ToGrasshopper", args);
    //}

  }

  

}

