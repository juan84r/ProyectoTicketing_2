import { useState } from "react";
import { useNavigate } from "react-router-dom";
import fondo from "./assets/fondo.jpg";

function Register() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  const isValidEmail = (email) => {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
  };

  const handleRegister = async () => {
    setError("");
    setSuccess("");

    if (!isValidEmail(email)) {
      setError("Ingresá un correo válido (ej: usuario@gmail.com)");
      return;
    }

    if (password.length < 4) {
      setError("La contraseña debe tener al menos 4 caracteres");
      return;
    }

    try {
      setLoading(true);

      const res = await fetch(
        "http://localhost:5171/api/v1/auth/register",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify({
            email,
            password,
            role: "User"
          })
        }
      );

      if (!res.ok) {
        setError(
          "Error al registrarse. Posiblemente el usuario ya existe."
        );
        return;
      }

      setSuccess("Usuario creado correctamente");

      setTimeout(() => {
        navigate("/");
      }, 1500);
    }
    catch (error)
    {
      console.error(error);
      setError("Error de conexión");
    }
    finally
    {
      setLoading(false);
    }
  };

  return (
    <div
      className="login-container"
      style={{
        backgroundImage: `url(${fondo})`,
        backgroundSize: "cover",
        backgroundPosition: "center",
        width: "100vw",
        height: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        position: "fixed",
        top: 0,
        left: 0
      }}
    >
      <div
        style={{
          position: "absolute",
          inset: 0,
          background: "rgba(0,0,0,0.6)"
        }}
      />

      <div
        className="login-card"
        style={{
          position: "relative",
          zIndex: 1
        }}
      >
        <h1
          style={{
            color: "white",
            marginBottom: "20px"
          }}
        >
          Crear cuenta
        </h1>

        {error && (
          <div
            style={{
              backgroundColor: "#e74c3c",
              color: "white",
              padding: "10px",
              borderRadius: "6px",
              marginBottom: "15px"
            }}
          >
            {error}
          </div>
        )}

        {success && (
          <div
            style={{
              backgroundColor: "#27ae60",
              color: "white",
              padding: "10px",
              borderRadius: "6px",
              marginBottom: "15px"
            }}
          >
            {success}
          </div>
        )}

        <div className="input-group">
          <input
            placeholder="Correo electrónico"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            style={{
              padding: "10px",
              margin: "10px 0",
              width: "100%",
              borderRadius: "5px",
              border: "none",
              boxSizing: "border-box"
            }}
          />
        </div>

        <div className="input-group">
          <input
            type="password"
            placeholder="Contraseña"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            style={{
              padding: "10px",
              margin: "10px 0",
              width: "100%",
              borderRadius: "5px",
              border: "none",
              boxSizing: "border-box"
            }}
          />
        </div>

        <button
          className="login-btn"
          onClick={handleRegister}
          disabled={loading}
          style={{
            padding: "10px 20px",
            width: "100%",
            backgroundColor: "#2ed573",
            color: "white",
            border: "none",
            borderRadius: "5px",
            cursor: "pointer",
            fontWeight: "bold",
            marginTop: "10px"
          }}
        >
          {loading ? "Registrando..." : "Registrarse"}
        </button>

        <div
          className="register-link"
          style={{
            marginTop: "20px",
            color: "white"
          }}
        >
          ¿Ya tenés cuenta?{" "}
          <span
            onClick={() => navigate("/")}
            style={{
              color: "#00a8ff",
              cursor: "pointer",
              fontWeight: "bold",
              textDecoration: "underline"
            }}
          >
            Iniciá sesión
          </span>
        </div>
      </div>
    </div>
  );
}

export default Register;