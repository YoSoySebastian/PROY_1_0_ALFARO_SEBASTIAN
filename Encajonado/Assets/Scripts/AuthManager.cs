using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TMP_InputField nameInputField;
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_Text statusText;

    [Header("Configuración")]
    public string gameSceneName = "GameScene";
    private Queue<string> statusQueue = new Queue<string>();
    private bool shouldLoadScene = false;

    void Start()
    {
        // auth = FirebaseAuth.DefaultInstance;
        // dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        lock (statusQueue)
        {
            if (statusQueue.Count > 0) statusText.text = statusQueue.Dequeue();
        }

        if (shouldLoadScene)
        {
            shouldLoadScene = false;
            SceneManager.LoadScene(gameSceneName);
        }
    }

    public void RegisterUser()
    {
        EnqueueStatus("Modo offline: Conexión con Firebase deshabilitada por seguridad.");
        /*
        auth.CreateUserWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text).ContinueWith(task => {
            if (task.IsFaulted) EnqueueStatus("Error: " + task.Exception.GetBaseException().Message);
            else
            {
                dbReference.Child("users").Child(task.Result.User.UserId).Child("name").SetValueAsync(nameInputField.text);
                EnqueueStatus("¡Cuenta creada! Ya puedes iniciar sesión.");
            }
        });
        */
    }

    public void LoginUser()
    {
        EnqueueStatus("Accediendo en modo local...");
        shouldLoadScene = true;

        /*
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsFaulted) EnqueueStatus("Error: Datos incorrectos");
            else
            {
                EnqueueStatus("¡Bienvenido! Entrando...");
                shouldLoadScene = true; 
            }
        });
        */
    }

    private void EnqueueStatus(string message)
    {
        lock (statusQueue) { statusQueue.Enqueue(message); }
    }

    public void GuardarPasosFirebase(int nivel, int pasosNuevos)
    {
        Debug.Log("Sincronización deshabilitada: No se enviará el récord a Firebase.");
    }

    public void ObtenerRecordFirebase(int nivel, System.Action<int> callback)
    {
        callback(0);
    }
}