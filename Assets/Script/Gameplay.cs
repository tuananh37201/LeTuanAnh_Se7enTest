using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameplay : MonoBehaviour
{
    public CameraController cameraController;
    public Transform player;
    public GameObject[] goals;

    private GameObject nearestBall;
    private GameObject farthestBall;
    private GameObject nearestGoal;
    private float nearestGoalDistance;

    private void Update()
    {
        FindNearestBall();
        FindFarthestBall();
    }

    // Tìm bóng gần nhất so với người chơi
    private GameObject FindNearestBall()
    {
        float minDistance = Mathf.Infinity;

        // duyệt qua các quả bóng có tag là "Ball"
        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            float distance = Vector3.Distance(ball.transform.position, player.position);

            // nếu khoảng cách từ quả bóng tới người chơi nhỏ hơn minDistance thì gán nó là nearestBall
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestBall = ball;
            }
        }
        return nearestBall;
    }

    // Tìm bóng xa nhất so với người chơi
    private GameObject FindFarthestBall()
    {
        float maxDistance = 0f;
        // duyệt qua các quả bóng có tag là "Ball"
        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            float distance = Vector3.Distance(ball.transform.position, player.position);

            // nếu khoảng cách từ quả bóng tới người chơi lớn hơn maxDistance thì gán nó là farthestBall
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestBall = ball;
            }
        }
        return farthestBall;
    }

    // Tìm khung thành gần nhất so với bóng tương tự như tìm Bóng gần nhất
    private GameObject FindNearestGoal(GameObject ball)
    {
        nearestGoal = null;
        nearestGoalDistance = Mathf.Infinity;

        foreach (GameObject goal in goals)
        {
            float distance = Vector3.Distance(ball.transform.position, goal.transform.position);
            if (distance < nearestGoalDistance)
            {
                nearestGoal = goal;
                nearestGoalDistance = distance;
            }
        }
        return nearestGoal;
    }

    // Hàm Camera chuyển mục tiêu
    IEnumerator SwitchTarget(GameObject ball)
    {
        // chuyển mục tiêu của camera sang bóng
        cameraController.target = ball.transform;
        // đợi đến khi bóng vào khung thành thì sau 2 giây chuyển camera về phía người chơi
        yield return new WaitUntil(() => ball.GetComponent<Ball>().isGoal);
        yield return new WaitForSeconds(2);
        cameraController.target = player;

    }
    
    // Xử lý sự kiện khi nhấn nút "Kick"
    public void OnKickButton()
    {
        // tìm khung thành gần nhất so với neaerstBall
        FindNearestGoal(nearestBall);
        // đổi camera sang nearestBall
        StartCoroutine(SwitchTarget(nearestBall));
        cameraController.target = nearestBall.transform;

        // xác định hướng sút về phía khung thành gần nhất
        Vector3 direction = nearestGoal.transform.position - nearestBall.transform.position;

        // tạo lực sút
        float force = Mathf.Clamp(10f / nearestGoalDistance, 1f, 10f);

        // sút
        nearestBall.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);
    }

    // Xử lý sự kiện khi nhấn nút "AutoKick"
    public void OnAutoKickButton()
    {
        // tìm khung thành gần nhất so với farthestBall
        FindNearestGoal(farthestBall);
        // đổi camera sang farthestBall
        StartCoroutine(SwitchTarget(farthestBall));
        cameraController.target = farthestBall.transform;

        // tạo lực sút về phía khung thành tương tự như nút Kick
        Vector3 direction = nearestGoal.transform.position - farthestBall.transform.position;
        float force = Mathf.Clamp(10f / nearestGoalDistance, 1f, 10f);

        farthestBall.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);
    }
    
    // Load lại scene
    public void OnResetButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
