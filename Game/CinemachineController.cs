using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    [SerializeField] private bool isQuest;
    private CinemachineFreeLook cam;
    private CinemachineFreeLook.Orbit[] originalOrbits;
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField, Range(1f, 1.5f)] private float minZoom=1f;
    [SerializeField, Range(1f, 2f)] private float maxZoom = 2f;
    [AxisStateProperty] public AxisState zAxis = new(0, 1, false, true, 50f, 0.1f, 0.1f, "Mouse ScrollWheel", false);

    private float targetFOV;
    private float initialFOV;
    private Coroutine fovTransitionCoroutine;

    //quest mode taro di main cam
    private ArenaManager arenaManager;
    private List<Cinemachine.CinemachineFreeLook> freeLookCamList;
    private int camIdx;
    private List<CinemachineFreeLook.Orbit[]> originalOrbitsList;

    #region Singleton
    private static CinemachineController instance;
    public static CinemachineController Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    
    private void ScrollZoom()
    {
        if (originalOrbits != null)
        {
            zAxis.Update(Time.deltaTime);
            float zoomScale = Mathf.Lerp(minZoom, maxZoom, zAxis.Value);
            for (int i = 0; i < originalOrbits.Length; i++)
            {
                cam.m_Orbits[i].m_Height = originalOrbits[i].m_Height * zoomScale;
                cam.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius * zoomScale;
            }
        }
    }

    private void FOVChange()
    {
        bool forwardKey = Input.GetKey(KeyCode.W);
        bool backwardKey = Input.GetKey(KeyCode.S);
        bool leftKey = Input.GetKey(KeyCode.A);
        bool rightKey = Input.GetKey(KeyCode.D);
        bool runKey = Input.GetKey(KeyCode.LeftShift);

        if (runKey && (forwardKey || backwardKey || leftKey || rightKey))
        {
            SetTargetFOV(45f);
        }
        else
        {
            SetTargetFOV(40f);
        }
    }

    private void SetTargetFOV(float fov)
    {
        if (targetFOV != fov)
        {
            if (fovTransitionCoroutine != null)
            {
                StopCoroutine(fovTransitionCoroutine);
            }

            targetFOV = fov;
            initialFOV = cam.m_Lens.FieldOfView;

            fovTransitionCoroutine = StartCoroutine(FOVTransitionCoroutine());
        }
    }

    private IEnumerator FOVTransitionCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            float currentFOV = Mathf.Lerp(initialFOV, targetFOV, t);
            cam.m_Lens.FieldOfView = currentFOV;
            yield return null;
        }

        cam.m_Lens.FieldOfView = targetFOV;
    }
    public void LockCamera()
    {
        cam.m_XAxis.m_InputAxisName = "";
        cam.m_YAxis.m_InputAxisName = "";
    }
    public void UnlockCamera()
    {
        cam.m_XAxis.m_InputAxisName = "Mouse X";
        cam.m_YAxis.m_InputAxisName = "Mouse Y";
    }

    private void CheckIdx()
    {
        camIdx = arenaManager.PlayerIdx;
    }

    private CinemachineFreeLook.Orbit[] GetOrbits(Cinemachine.CinemachineFreeLook c)
    {
        if (c == null) return null;

        var orbits = new CinemachineFreeLook.Orbit[c.m_Orbits.Length];
        for (int i = 0; i < orbits.Length; i++)
        {
            orbits[i].m_Height = c.m_Orbits[i].m_Height;
            orbits[i].m_Radius = c.m_Orbits[i].m_Radius;
        }
        return orbits;
    }

    private void GetOrbitsList()
    {
        foreach (var cam in freeLookCamList)
        {
            if (cam != null)
            {
                originalOrbitsList.Add(GetOrbits(cam));
            }
        }
    }

    private void Start()
    {
        originalOrbitsList = new List<CinemachineFreeLook.Orbit[]>();

        if (!isQuest)
        {
            cam = GetComponent<CinemachineFreeLook>();
            originalOrbits = GetOrbits(cam);
        }
        else if (isQuest)
        {
            arenaManager = ArenaManager.Instance;
            if (arenaManager != null)
            {
                freeLookCamList = arenaManager.FreeLookCameraList;
            }
            GetOrbitsList();
        }
    }

    void Update()
    {
        if (isQuest)
        {
            CheckIdx();
            if (arenaManager != null)
            {
                freeLookCamList = arenaManager.FreeLookCameraList;
            }
            if (freeLookCamList != null && cam != freeLookCamList[camIdx])
            {
                cam = freeLookCamList[camIdx];
                originalOrbits = originalOrbitsList[camIdx];
            }
        }
        ScrollZoom();
        FOVChange();
    }
}
