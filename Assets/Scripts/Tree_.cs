using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_ : MonoBehaviour
{
    [SerializeField] private GameObject branchPrefab;

    [Header("other")] [SerializeField] private float rootLenght = 4;

    [SerializeField] private float currentLenght = -1;

    [SerializeField] private float reductionLenghtPerLevel = 0.1f;

    [SerializeField] private int recursionDepht = 2;

    private int currentDepth = 0;

    private Queue<GameObject> frontier = new Queue<GameObject>();   

    private void Start()
    {
        currentLenght = rootLenght;

        GameObject root = Instantiate(branchPrefab,transform);
        SetBranchLenght(root, rootLenght);

        frontier.Enqueue(root);
        GenerateTree();
    }

    #region Recursion Base
    // Recursión: Una función que se llama a sí misma, es una alternativa de usar un bucle y limitar el numero de llamadas.
    private void CallMe(int i)
    {
        if (i == 4) return;
        Debug.Log(i);
        i++;
        CallMe(i);
    }

    #endregion

    private void GenerateTree()
    {
        if (currentDepth >= recursionDepht) return;
        ++currentDepth;
        rootLenght = rootLenght - rootLenght * reductionLenghtPerLevel;

        List<GameObject> levelNodes = new List<GameObject>();

        while (frontier.Count > 0)
        {
            var branch = frontier.Dequeue();

            GameObject leftBranch = GenerateBranch(branch, Random.Range(10f,30f));
            GameObject rightBranch = GenerateBranch(branch, -Random.Range(10f, 30f));

            levelNodes.Add(leftBranch);
            levelNodes.Add(rightBranch);
        }

        foreach (GameObject node in levelNodes)
        {
            frontier.Enqueue(node);
        }

        GenerateTree();
    }

    private GameObject GenerateBranch(GameObject prevBranch, float angle)
    {
        GameObject branch = Instantiate(branchPrefab, transform);
        
        branch.transform.position = prevBranch.transform.position + prevBranch.transform.up * currentLenght;
        Quaternion prevRotation = prevBranch.transform.rotation;

        SetBranchLenght(branch, currentLenght);
        prevRotation *= Quaternion.Euler(0, 0, angle);  
        branch.transform.rotation = prevRotation;
       
        return branch;
    }
    private void SetBranchLenght(GameObject branch, float lenght)
    {
        Transform line = branch.transform.GetChild(0);
        Transform circle = branch.transform.GetChild(1);
        line.localScale = new Vector3(line.localScale.x, lenght, line.localScale.z);
        line.localPosition = new Vector3(0f, lenght * 0.5f, 0f);
        circle.localPosition = new Vector3(0f, lenght, 0f);
    }
    private float GetBranchLength(GameObject branch)
    {
        Transform line = branch.transform.GetChild(0);
        return line.localScale.y;
    }
}
