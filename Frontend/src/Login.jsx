import { useState } from "react";
import { useNavigate } from "react-router-dom"; 
import "./Login.css";
import fondo from "./assets/fondo.jpg";

function Login({ onLogin }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate(); 
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

const handleLogin = async () => {
  try {
    setLoading(true);
    setError("");

    const response = await fetch(
      "http://localhost:5171/api/v1/auth/login",
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      }
    );

    if (!response.ok) {
      setError("Credenciales incorrectas");
      return;
    }

    const data = await response.json();

    localStorage.setItem("userId", data.userId);
    localStorage.setItem("userRole", data.role);

    if (data.role === "Admin") {
      navigate("/admin");
    } else {
      navigate("/eventos");
    }

    if (onLogin) onLogin();
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
    <div className="login-container" style={{
        backgroundImage: `url(${fondo})`, backgroundSize: "cover", width: "100vw", height: "100vh",
        display: "flex", justifyContent: "center", alignItems: "center", position: "fixed", top: 0, left: 0,
      }}>
      <div style={{ position: "absolute", inset: 0, background: "rgba(0,0,0,0.6)" }} />
      <div className="login-card" style={{ position: 'relative', zIndex: 1 }}>
        <h1>¡Bienvenido!</h1>
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
        <div className="input-group">
          <input type="email" placeholder="Correo" value={email} onChange={(e) => setEmail(e.target.value)} />
        </div>
        <div className="input-group">
          <input type="password" placeholder="Contraseña" value={password} onChange={(e) => setPassword(e.target.value)} />
        </div>
        <button className="login-btn" onClick={handleLogin}disabled={loading}>{loading ? "Ingresando..." : "Login"}</button>
        <div className="register-link">
          ¿No tenés cuenta? <span onClick={() => navigate("/register")} style={{cursor:'pointer', color:'#3498db'}}>Registrate</span>
        </div>
      </div>
    </div>
  );
} 

export default Login;