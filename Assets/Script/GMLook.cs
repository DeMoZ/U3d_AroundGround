using UnityEngine;
using System.Collections;

public class GMLook : MonoBehaviour// ControlGetter//MonoBehaviour// ControlGetter
{
    /*
    bool _invertMouseX = false;
    float _invertX = 1;                              // если буду инвертировать, то значение -1

    bool _invertMouseY = true;              // инвертировать ли мышь по У оси 
    float _invertY = 1;                              // если буду инвертировать, то значение -1

    public Vector2 _swipeDelta = new Vector2(0, 0);
    Vector3 _nextPos = new Vector3(0, 0, 0);                        // текущее положение цели (после приметения Smooth или без него)
    [Tooltip("положение камеры относительно тела")]
    public Vector3 _camPosOffset = new Vector3(0.8f, 1.5f, -2);
    //public float _behindTarget = -2f;                // расстояние на котором камера нормально держится за игроком
    // public float _aboveTarget = 1f;                  // высота над игроком?(можно вставить сюда _body.localScale.y/2 цели)
    //  public float _asideTarget = 3f;                  // смещение вправо или влево(-1) от игрока    
    float _camOffsetXCurrent;
    float _camOffsetXCurrentCollisioned;
    float _camOffsetZCurrent;                      //1 измененное расстояние камеры
    float _camOffsetZCurrentCollisioned;           //2 измененное растояние учитывающее столкновение с обьектами

    LayerMask _botLayer = 1 << 9;


    public float _camRotHirizontSpeed = 16f;               // скорость вращения камерой по горизонтали
    public float _camRotVerticalSpeed = 6f;               // скорость вращения камерой по вертикали
                                                          // public float _camFollowSpeed = 5;               // скорость преследования цели камерой, когда
    public float _camScrollStep = 5;              // скорость приближения/удаления камеры

    public float sensitivityOX = 15F;//чувствирельность управления мыши на компе
    public float sensitivityOY = 15F;

    public float minimumY = -85f;                   // ОТРИЦАТЕЛЬНОЕ ЗНАЧЕНИЕ. ограничение вращение камеры вниз
    public float maximumY = 85f;                    // ограничение вращение камеры вверх

    float _mouseScroll = 0;                          // колесо мышы для приближения/удаления камеры
    bool _smoothY = false;                           // плавное преследование по оси Y
    float _smoothYspeed = 10;                       // скорость движения камеры по оси Y
    Vector3 _countPos = new Vector3(0, 0, 0);                          // вспомогательная переменная, для подсчетка координат цели

    public CamPosType _camPosType = CamPosType.offset;
    public enum CamPosType
    {
        behind,
        offset,
        properWallsNOTfINISHED
    }

    GMControlDriver _cd;
    Transform _body;
    Transform _camera;
    //BodyPrefs _bp;

    void Awake()
    {
        _cd = GetComponent<GMControlDriver>();
//!!!!!!!!!!!        _body = _cd._body;
        // _bp = _body.GetComponent<BodyPrefs>();

        _camera = _cd._camera;


    }
    void Start()
    {
        _camOffsetZCurrent = _camPosOffset.z;
        if (_camera != null)// применяем положение таргета(чтобы сразу начать приближенно)
            _nextPos = _camera.position;
        else Debug.LogError("на стратре камеру не нашел");
    }

    void LateUpdate()
    {
        if (_body != _cd._body)
        {
 //!!!!!!!!!!           _body = _cd._body;
            //_bp = _body.GetComponent<BodyPrefs>();
        }


        if (_body == null && _camera == null)
        {
            return;
        }
        else if (_body == null && _camera != null)// выхожу из управления камерой, если нету трансформа камеры
        {
            //свободный полет камеры( создать невидимую сферу с коллайдером и двигать ее)
            //но пока отбой
            return;
        }
        else if (_body != null && _camera == null)
        {
            // тело есть, а камеры нет...... что делать?
        }

        if (_body != null)
        {
            //просчитать точку , удаленную от тела вправо в координатах камеры
            //_bodyPos=_body.position
        }

        _invertX = (_invertMouseX) ? 1 : -1;
        _invertY = (_invertMouseY) ? 1 : -1;

        //_swipeDelta.x =_cd.GetMouse("Horizontal") * _invertX* _camRotHirizontSpeed * Time.deltaTime;
        //_swipeDelta.y = _cd.GetMouse("Vertical") * _invertY* _camRotVerticalSpeed * Time.deltaTime * 2;
        //_mouseScroll = _cd.GetMouse("MouseWheel");

        _swipeDelta.x = Mathf.Lerp(_swipeDelta.x, _cd.GetMouse("Horizontal") * _invertX, _camRotHirizontSpeed * Time.deltaTime);
        _swipeDelta.y = Mathf.Lerp(_swipeDelta.y, _cd.GetMouse("Vertical") * _invertY, _camRotVerticalSpeed * Time.deltaTime * 2);
        _mouseScroll = _cd.GetMouse("MouseWheel");

        switch (_camPosType)
        {
            case CamPosType.behind:
                _nextPos.x = _body.position.x;
                if (_smoothY)   // плавное приследование по Y
                {
                    _countPos = Vector3.Lerp(new Vector3(0, _nextPos.y, 0), new Vector3(0, _body.position.y, 0), _smoothYspeed * Time.deltaTime);
                    _nextPos.y = _countPos.y;
                }
                else _nextPos.y = _body.position.y;
                _nextPos.z = _body.position.z;
                RotationWithTouch();

                // CameraPositionOffset();


                break;
            case CamPosType.offset:

                _nextPos.x = _body.position.x;
                if (_smoothY)   // плавное приследование по Y
                {
                    _countPos = Vector3.Lerp(new Vector3(0, _nextPos.y, 0), new Vector3(0, _body.position.y, 0), _smoothYspeed * Time.deltaTime);
                    _nextPos.y = _countPos.y;
                }
                else _nextPos.y = _body.position.y;
                _nextPos.z = _body.position.z;
                RotationWithTouch();

                CameraPositionOffset();

                break;
            case CamPosType.properWallsNOTfINISHED:
                _nextPos.x = _body.position.x;
                if (_smoothY)   // плавное приследование по Y
                {
                    _countPos = Vector3.Lerp(new Vector3(0, _nextPos.y, 0), new Vector3(0, _body.position.y, 0), _smoothYspeed * Time.deltaTime);
                    _nextPos.y = _countPos.y;
                }
                else _nextPos.y = _body.position.y;
                _nextPos.z = _body.position.z;
                RotationWithTouch();

                CameraPositionProperWalls();


                break;
        }
    }
    void CameraPositionProperWalls()
    {
        //float swipeDeltaYClamped = ClampAngle(_swipeDelta.y + _camera.eulerAngles.x, minimumY, maximumY);       // ok

        //Quaternion rotation = Quaternion.Euler(swipeDeltaYClamped, _camera.eulerAngles.y + _swipeDelta.x, 0);   // ok

        //_camOffsetZCurrent = _camOffsetZCurrent + _mouseScroll * _camScrollStep;                                // ok скрол мышкой
        //_camOffsetZCurrent = Mathf.Clamp(_camOffsetZCurrent, _camPosOffset.z * 2, -_camPosOffset.z * 2);        // ok скорол мышкой
        // говно      

    }
    Vector3 AnotherCollision(Vector3 myPosition, Vector3 targetPosition, float currentDistance, float collisionDistance)
    {
        Ray rayCollision = new Ray(targetPosition, myPosition - targetPosition);// + new Vector3(_camPosOffset.x, 0, 0));
        RaycastHit hitCollision;
        if (Physics.Raycast(rayCollision, out hitCollision, collisionDistance))//,layerMask
        {//сохраняем z камеры равный расстоянию до обьекта столкновения
         //  Debug.Log("Столкновение");
            if (hitCollision.distance < currentDistance * -1f)
            {
                //    Debug.Log("Столкновение ПРАВИТСZ");
                // Debug.DrawRay(targetPosition, hitCollision.point - targetPosition, Color.white);
                currentDistance = hitCollision.distance;
                currentDistance = currentDistance * -1f;
            }
        }

        return Vector3.zero;// currentDistance;
    }
    void CameraPositionOffset()
    {
        // Debug.Log("офсет работает "+_swipeDelta);
        // сносный модуль
        float swipeDeltaYClamped = ClampAngle(_swipeDelta.y + _camera.eulerAngles.x, minimumY, maximumY);       // ok

        Quaternion rotation = Quaternion.Euler(swipeDeltaYClamped, _camera.eulerAngles.y + _swipeDelta.x, 0);   // ok

        _camOffsetZCurrent = _camOffsetZCurrent + _mouseScroll * _camScrollStep;                                // ok скрол мышкой
        _camOffsetZCurrent = Mathf.Clamp(_camOffsetZCurrent, _camPosOffset.z * 2, -_camPosOffset.z * 2);        // ok скорол мышкой

        // целевое положение с добавленной высотой, ограниченное по углам вращения по Y
        Vector3 targetPosFixed = new Vector3(_nextPos.x, _nextPos.y + _camPosOffset.y, _nextPos.z);
        targetPosFixed += _camera.TransformDirection(_camPosOffset.x, 0, 0);    // добавляю офсет по х

        _camOffsetZCurrentCollisioned = _camOffsetZCurrent;
        Vector3 cameraColliderPoint = _camera.TransformPoint(new Vector3(0, 0, -1));//!!!!!!

        _camOffsetZCurrentCollisioned = CheckCamCollision(cameraColliderPoint, targetPosFixed, _camOffsetZCurrent, Vector3.Distance(cameraColliderPoint, targetPosFixed));
        Vector3 position = rotation * new Vector3(0, 0, _camOffsetZCurrentCollisioned) + targetPosFixed;
        _camera.rotation = rotation;
        _camera.position = position;
    }

    void RotationWithTouch()
    {
        float swipeDeltaYClamped = ClampAngle(_swipeDelta.y + _camera.eulerAngles.x, minimumY, maximumY);
        Quaternion rotation = Quaternion.Euler(swipeDeltaYClamped, _camera.eulerAngles.y + _swipeDelta.x, 0);
        _camOffsetZCurrent = _camOffsetZCurrent + _mouseScroll * _camScrollStep;
        _camOffsetZCurrent = Mathf.Clamp(_camOffsetZCurrent, _camPosOffset.z * 2, -_camPosOffset.z * 2);

        Vector3 targetPosFixed = new Vector3(_nextPos.x, _nextPos.y + _camPosOffset.y, _nextPos.z);

        _camOffsetZCurrentCollisioned = _camOffsetZCurrent;
        Vector3 cameraColliderPoint = _camera.TransformPoint(new Vector3(0, 0, -1));
        _camOffsetZCurrentCollisioned = CheckCamCollision(cameraColliderPoint, targetPosFixed, _camOffsetZCurrent, Vector3.Distance(cameraColliderPoint, targetPosFixed));
        Vector3 position = rotation * new Vector3(0, 0, _camOffsetZCurrentCollisioned) + targetPosFixed;
        _camera.rotation = rotation;
        _camera.position = position;
    }
    float ClampAngle(float angle, float min, float max)
    {
        // иногда проскакивает значение угла ниже 0 и выше 360. это правлю
        if (angle < 0) angle += 360;
        if (angle > 360) angle -= 360;
        // Debug.Log("angle<360 = " + angle);
        // запретить камере быть ниже  360+minimumY (минимум должен быть отрицательным)
        // запретить камере быть выше maximumY
        if (angle > 180)
            return Mathf.Clamp(angle, 360 + min, 360);
        else
            return Mathf.Clamp(angle, min, max);
    }
    float CheckCamCollision(Vector3 myPosition, Vector3 targetPosition, float currentDistance, float collisionDistance)
    {
        Ray rayCollision = new Ray(targetPosition, myPosition - targetPosition);// + new Vector3(_camPosOffset.x, 0, 0));
        RaycastHit hitCollision;
        if (Physics.Raycast(rayCollision, out hitCollision, collisionDistance))//,layerMask
        {//сохраняем z камеры равный расстоянию до обьекта столкновения
            if (hitCollision.transform.gameObject.layer != _botLayer)
                if (hitCollision.distance < currentDistance * -1f)
                {
                    //    Debug.Log("Столкновение ПРАВИТСZ");
                    // Debug.DrawRay(targetPosition, hitCollision.point - targetPosition, Color.white);
                    currentDistance = hitCollision.distance;
                    currentDistance = currentDistance * -1f;
                }
        }

        return currentDistance;
    }

    void TestRotate()
    {
        //Transform angle in degree in quaternion form used by Unity for rotation.
        Quaternion rotation = Quaternion.Euler(_camera.eulerAngles.x + _swipeDelta.y, _camera.eulerAngles.y + _swipeDelta.x, 0.0f);

        //The new position is the target position + the distance vector of the camera
        //rotated at the specified angle.
        Vector3 position = rotation * new Vector3(0, 0, -2) + _body.position;

        //Update the rotation and position of the camera.
        _camera.rotation = rotation;
        _camera.position = position;

    }
    */
}
