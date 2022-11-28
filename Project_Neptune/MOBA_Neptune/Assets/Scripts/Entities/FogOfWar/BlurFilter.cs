using UnityEngine;

[ExecuteInEditMode]
public class BlurFilter : MonoBehaviour
{
    [SerializeField, Range(1, 30)]
    int _iteration = 4;

    [SerializeField, Range(1, 8)] 
    int upscaleTexture = 4;

    [SerializeField] 
    private bool _useBlur;
    public Material _material;
    public Material _material1;

    public RenderTexture myTexture;

    RenderTexture rt1;
    RenderTexture rt2;
    //RenderTexture save;

    private void Start()
    {
        rt1 = RenderTexture.GetTemporary(myTexture.width * upscaleTexture, myTexture.height * upscaleTexture);
        rt2 = RenderTexture.GetTemporary(myTexture.width * upscaleTexture, myTexture.height * upscaleTexture);
        //save = RenderTexture.GetTemporary(myTexture.width, myTexture.height);
    }

    void Update()
    {
        if (_useBlur)
        {
            Graphics.Blit(myTexture, rt1, _material, 0);
            
            //Can Sample the Number Of Iteration
            for (var i = 0; i < _iteration; i++)
            {
                Graphics.Blit(rt1, rt2, _material, 1);
                Graphics.Blit(rt2, rt1, _material, 2);
            }
            _material1.SetTexture("_FogTex0", rt1);
            RenderTexture.ReleaseTemporary(rt1);
            RenderTexture.ReleaseTemporary(rt2);
        }
        else
        {
            Graphics.Blit(myTexture, rt1, _material1, 0);
            Graphics.Blit(rt1, myTexture, _material1, 0 );
            _material1.SetTexture("_FogTex0", rt1);
            RenderTexture.ReleaseTemporary(rt1);
        }
    }
}