using System;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    [Header("额外逻辑")]
    public string textName;

    public string textFront;


    [Header("主要逻辑")]

    //定义图片保存路径
    private string m_ShotPath;

    [Tooltip("这个不是Main相机，其他摄像机，默认不激活它")]
    //这个不是Main相机，其他摄像机，默认不激活它
    public Camera CameraTrans;

    //显示图片
    public RawImage image;

    void Start()
    {
        DateTime dt = DateTime.Now;
        //初始化路径，实际使用应该用Application.persistentDataPath，
        //因为使用dataPath就是Asset文件不能读写操作
        m_ShotPath = Application.dataPath + "/Shoot/";
        if (!Directory.Exists(m_ShotPath))
        {
            Directory.CreateDirectory(m_ShotPath);
        }
        // m_partShotPath = Application.streamingAssetsPath + "/PartScreenShot.png";
        // m_OtherCameraPath = Application.streamingAssetsPath + "/OtherCameraScreenShot.png";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CaptureByUnity();
            //AssetDatabase.Refresh();
        }
    }

    // [Button("全屏截图")]
    /// <summary>
    /// 使用Application类下的CaptureScreenshot()方法实现截图
    /// 优点：简单，可以快速地截取某一帧的画面、全屏截图
    /// 缺点：不能针对摄像机截图，无法进行局部截图
    /// </summary>
    /// <param name="mFileName">M file name.</param>
    private void CaptureByUnity()
    {
        // ScreenCapture.CaptureScreenshot("Shoot/" + textFront + "_" + textName + ".png", 0);
        ScreenCapture.CaptureScreenshot
            (m_ShotPath + "/FullShoot_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".png", 0);
        // ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/FullScreenShot.png", 0);


        textName = "";
    }

    /// <summary>
    /// 根据一个Rect类型来截取指定范围的屏幕, 左下角为(0,0)
    /// 读取屏幕像素存储为纹理图片
    /// </summary>
    /// <param name="mRect">M rect.截屏的大小</param>
    /// <param name="mFileName">M file name.保存路径</param>
    private IEnumerator CaptureByRect(Rect mRect, string mFileName)
    {
        //等待渲染线程结束
        yield return new WaitForEndOfFrame();
        //初始化Texture2D, 大小可以根据需求更改
        Texture2D mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.RGB24, false);
        //读取屏幕像素信息并存储为纹理数据
        mTexture.ReadPixels(mRect, 0, 0);
        //应用
        mTexture.Apply();
        //将图片信息编码为字节信息
        byte[] bytes = mTexture.EncodeToPNG();
        //保存
        System.IO.File.WriteAllBytes(mFileName, bytes);
        //需要展示次截图，可以返回截图,或者使用out修饰参数也可以带出去
        //return mTexture;
    }

    /// <summary>
    /// 指定相机截图
    /// </summary>
    /// <returns>The by camera.</returns>
    /// <param name="mCamera">M camera.要被截屏的相机</param>
    /// <param name="mRect">M rect. 截屏的区域</param>
    /// <param name="mFileName">M file name.</param>
    private IEnumerator CaptureByCamera(Camera mCamera, Rect mRect, string mFileName)
    {
        //等待渲染线程结束
        yield return new WaitForEndOfFrame();
        //初始化RenderTexture   深度只能是【0、16、24】截不全图请修改
        RenderTexture mRender = new RenderTexture((int)mRect.width, (int)mRect.height, 16);
        //设置相机的渲染目标
        mCamera.targetTexture = mRender;
        //开始渲染
        mCamera.Render();
        //激活渲染贴图读取信息
        RenderTexture.active = mRender;
        Texture2D mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.RGB24, false);
        //读取屏幕像素信息并存储为纹理数据
        mTexture.ReadPixels(mRect, 0, 0);
        //应用
        mTexture.Apply();
        //释放相机，销毁渲染贴图
        mCamera.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(mRender);
        //将图片信息编码为字节信息
        byte[] bytes = mTexture.EncodeToPNG();
        //保存
        System.IO.File.WriteAllBytes(mFileName, bytes);
        //需要展示次截图，可以返回截图
        //return mTexture;
    }
}