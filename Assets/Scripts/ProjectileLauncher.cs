using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform launchPoint;
    public GameObject projectilePrefab;

  

    public void FireProjectile()
    {
       

        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
        Vector3 origScale = projectile.transform.localScale;

        projectile.transform.localScale = new Vector3(
          (origScale.x * transform.localScale.x > 0) ? 1 : -1,
          origScale.y,
          origScale.z
        );

        if (transform.localScale.x < 0)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 0, -225);
        }
        else
        {
            projectile.transform.rotation = Quaternion.Euler(0, 0, 225);
        }
    }



}
