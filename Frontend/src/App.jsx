import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom' 
import './App.css'
import Login from "./Login";
import Register from "./Register";

function App() {
  const { eventId } = useParams();
  const navigate = useNavigate();
  
  // --- ESTADOS PRINCIPALES ---
  const [seats, setSeats] = useState([]);
  const [selectedSeats, setSelectedSeats] = useState([]);
  const [sectors, setSectors] = useState([]);
  const [sectorId, setSectorId] = useState(null);
  const [isLogged, setIsLogged] = useState(!!localStorage.getItem("userId"));
  const [view, setView] = useState("login");

  // --- ESTADOS DEL BLOQUEO Y RELOJ ---
  const [isPaying, setIsPaying] = useState(false);
  const [timeLeft, setTimeLeft] = useState(300); 
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);

  // 1. CARGAR SECTORES DINAMICAMENTE
  useEffect(() => {
    if (isLogged && eventId) {
      fetch(`http://localhost:5171/api/v1/events/${eventId}/sectors`)
        .then(res => res.json())
        .then(data => {
          setSectors(data);
          if (data && data.length > 0) {
            setSectorId(data[0].id);
          }
        })
        .catch(err => console.error("Error cargando sectores:", err));
    }
  }, [eventId, isLogged]);

  // 2. CARGAR ASIENTOS (CON AUTOREFRESH ININTERRUMPIDO)
  const loadSeats = () => {
    if (!sectorId) return;
    fetch(`http://localhost:5171/api/v1/events/${sectorId}/seats`)
      .then(res => res.json())
      .then(data => {
        setSeats(data);
      })
      .catch(err => console.error("Error cargando asientos:", err));
  };

  useEffect(() => {
    if (isLogged && sectorId) {
      // Carga inicial inmediata
      loadSeats();

      // SOLUCION: El intervalo corre SIEMPRE cada 4 segundos para escuchar cambios de otros usuarios
      const interval = setInterval(() => {
        loadSeats();
      }, 4000);

      return () => clearInterval(interval);
    }
  }, [sectorId, isLogged]); 

  // --- LOGICA DEL CRONOMETRO ---
  useEffect(() => {
    let timer;
    if (isPaying && timeLeft > 0) {
      timer = setInterval(() => setTimeLeft(prev => prev - 1), 1000);
    } else if (timeLeft === 0) {
      handleCancelPayment(); 
    }
    return () => clearInterval(timer);
  }, [isPaying, timeLeft]);

  const formatTime = (seconds) => {
    const m = Math.floor(seconds / 60);
    const s = seconds % 60;
    return `${m}:${s < 10 ? '0' : ''}${s}`;
  };

  // --- LOGICA DE INTERACCION ---
  const toggleSeat = (seatId, status) => {
    if (status !== 'Available' || isPaying) return;
    setSelectedSeats(prev => 
      prev.includes(seatId) ? prev.filter(id => id !== seatId) : [...prev, seatId]
    );
  };

  // PASO 1: BLOQUEAR ASIENTOS (CORREGIDO PARA ENVIAR ARRAY AL BACKEND)
  const handleConfirm = async () => {
    setError("");
    setSuccess("");
    const userId = localStorage.getItem("userId");

    if (selectedSeats.length === 0) return;

    try {
      setLoading(true);

      // CORRECCION: Modificamos el cuerpo para enviar "seatIds" como array exacto
      const response = await fetch("http://localhost:5171/api/v1/seats/lock", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          seatIds: selectedSeats, // <-- CAMBIO: Manda el array completo que espera C#
          userId: parseInt(userId)
        })
      });

      if (response.ok) {
        setIsPaying(true);
        setTimeLeft(300);
        return;
      }

      if (response.status === 409) {
        setError("Uno o más asientos ya fueron reservados por otro usuario.");
      } else {
        setError("Error al intentar congelar los asientos.");
      }

      setSelectedSeats([]);
      loadSeats();
    } catch (error) {
      setError("Error de conexión al bloquear asiento.");
    } finally {
      setLoading(false);
    }
  };

  // PASO 2: CANCELAR PAGO Y LIBERAR
  const handleCancelPayment = async () => {
    const userId = localStorage.getItem("userId");
    try {
      await fetch("http://localhost:5171/api/v1/seats/unlock", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ 
          seatIds: selectedSeats, // <-- CAMBIO: Consistente con array
          userId: parseInt(userId) 
        })
      });
      setIsPaying(false);
      setSelectedSeats([]);
      loadSeats();
    } catch (error) {
      setIsPaying(false);
    }
  };

  // PASO 3: FINALIZAR RESERVA DEFINITIVA
  const finalizarReserva = async () => {
    setError("");
    setSuccess("");
    const userId = localStorage.getItem("userId");

    try {
      setLoading(true);

      const response = await fetch("http://localhost:5171/api/v1/reservations", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          userId: parseInt(userId),
          seatIds: selectedSeats
        })
      });

      if (response.ok) {
        setSuccess("¡Compra realizada con éxito!");
        setIsPaying(false);
        setSelectedSeats([]);
        loadSeats();
        return;
      }

      if (response.status === 409) {
        setError("Otro usuario compró el asiento antes.");
      } else {
        setError("Error al procesar la compra.");
      }

      setSelectedSeats([]);
      loadSeats();
    } catch (error) {
      setError("Error de conexión al procesar la compra.");
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("userId");
    setIsLogged(false);
    setView("login");
    setSelectedSeats([]); 
  };

  if (!isLogged) {
    if (view === "login") return <Login onLogin={() => setIsLogged(true)} goToRegister={() => setView("register")} />;
    return <Register goToLogin={() => setView("login")} />;
  }

  return (
    <div style={{ padding: '20px', textAlign: 'center', backgroundColor: '#121212', color: 'white', minHeight: '100vh', position: 'relative' }}>
      
      <button onClick={() => navigate("/eventos")} style={navBtnStyle}>⬅ Volver</button>
      <button onClick={handleLogout} style={logoutBtnStyle}>Logout</button>

      <h1 style={{ color: '#ecf0f1', marginBottom: '30px' }}>Sistema de Ticketing</h1>
      
      {error && <div style={errorStyle}>{error}</div>}
      {success && <div style={successStyle}>{success}</div>}

      <div style={{ marginBottom: '20px', display: 'flex', gap: '15px', justifyContent: 'center', flexWrap: 'wrap' }}>
        {sectors.map(sector => (
          <button 
            key={sector.id}
            onClick={() => setSectorId(sector.id)}
            style={{
              padding: '10px 20px',
              backgroundColor: sectorId === sector.id ? '#3498db' : '#2c3e50',
              color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer', fontWeight: 'bold'
            }}>
            {sector.name}
          </button>
        ))}
      </div>

      <h3 style={{ color: '#bdc3c7' }}>{sectors.find(s => s.id === sectorId)?.name || 'Cargando...'}</h3>

      <div style={gridContainerStyle}>
        {seats.map(seat => {
          // El asiento esta ocupado si su estado no es Available
          const isOccupied = seat.status !== 'Available';
          
          return (
            <div 
              key={seat.id}
              onClick={() => toggleSeat(seat.id, seat.status)}
              style={{
                width: '45px', height: '45px', borderRadius: '6px', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '12px', fontWeight: 'bold', transition: '0.2s',
                cursor: !isOccupied && !isPaying ? 'pointer' : 'not-allowed',
                backgroundColor: selectedSeats.includes(seat.id) ? '#3498db' : (isOccupied ? '#c0392b' : '#27ae60'),
                transform: selectedSeats.includes(seat.id) ? 'scale(1.1)' : 'scale(1)',
                border: '1px solid rgba(255,255,255,0.1)'
              }}>
              {seat.seatNumber}
            </div>
          );
        })}
      </div>

      <div style={{ marginTop: '30px', display: 'flex', justifyContent: 'center', gap: '15px' }}>
        <button 
          disabled={selectedSeats.length === 0 || isPaying}
          onClick={() => setSelectedSeats([])}
          style={{ ...actionBtnStyle, backgroundColor: selectedSeats.length > 0 ? '#e74c3c' : '#7f8c8d' }}>
          Limpiar Selección
        </button>

        <button 
          disabled={selectedSeats.length === 0 || isPaying}
          onClick={handleConfirm}
          style={{ ...actionBtnStyle, backgroundColor: selectedSeats.length > 0 ? '#2ecc71' : '#7f8c8d' }}>
          {loading ? "Procesando..." : `Confirmar Reserva (${selectedSeats.length})`}
        </button>
      </div>

      {isPaying && (
        <div style={overlayStyle}>
          <div style={modalStyle}>
            <h2 style={{ color: '#e74c3c', marginBottom: '10px' }}>⏱️ Reserva Temporal</h2>
            <p style={{ fontSize: '1.4rem', margin: '20px 0' }}>Tiempo restante: <strong>{formatTime(timeLeft)}</strong></p>
            <p style={{ color: '#bdc3c7' }}>Estás reservando {selectedSeats.length} asiento(s).</p>
            
            <div style={{ marginTop: '30px', display: 'flex', gap: '15px', justifyContent: 'center' }}>
              <button onClick={finalizarReserva} style={pagoBtnStyle}>Finalizar Pago</button>
              <button onClick={handleCancelPayment} style={cancelBtnStyle}>Cancelar y Liberar</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

// Estilos limpios y organizados
const errorStyle = { backgroundColor: "#e74c3c", color: "white", padding: "10px", borderRadius: "6px", marginBottom: "20px", maxWidth: "600px", margin: "0 auto 20px auto" };
const successStyle = { backgroundColor: "#27ae60", color: "white", padding: "10px", borderRadius: "6px", marginBottom: "20px", maxWidth: "600px", margin: "0 auto 20px auto" };
const navBtnStyle = { position: 'absolute', top: '20px', left: '20px', padding: '8px 15px', backgroundColor: '#34495e', color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer' };
const logoutBtnStyle = { position: "absolute", top: "20px", right: "20px", padding: "8px 15px", backgroundColor: "#e74c3c", color: "white", border: "none", borderRadius: "5px", cursor: "pointer", fontWeight: 'bold' };
const gridContainerStyle = {display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(45px, 1fr))', gap: '10px', width: '100%', maxWidth: '700px', margin: '20px auto 0 auto', backgroundColor: '#2c3e50', padding: '20px', borderRadius: '12px'};
const actionBtnStyle = { padding: '12px 25px', color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer', fontWeight: 'bold' };
const overlayStyle = { position: 'fixed', top: 0, left: 0, right: 0, bottom: 0, backgroundColor: 'rgba(0,0,0,0.92)', display: 'flex', justifyContent: 'center', alignItems: 'center', zIndex: 2000 };
const modalStyle = {backgroundColor: '#2c3e50', padding: '30px', borderRadius: '25px', textAlign: 'center', border: '2px solid #f1c40f', boxShadow: '0 0 30px rgba(0,0,0,0.5)', width: '90%', maxWidth: '450px'};
const pagoBtnStyle = { padding: '12px 25px', backgroundColor: '#2ecc71', color: 'white', border: 'none', borderRadius: '8px', cursor: 'pointer', fontWeight: 'bold' };
const cancelBtnStyle = { padding: '12px 25px', backgroundColor: '#e74c3c', color: 'white', border: 'none', borderRadius: '8px', cursor: 'pointer', fontWeight: 'bold' };

export default App;