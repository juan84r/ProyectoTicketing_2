import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom' 
import './App.css'
import Login from "./Login";
import Register from "./Register";

function App() {
  const { eventId } = useParams(); // Obtenemos el ID del evento de la URL
  const navigate = useNavigate();
  
  const [seats, setSeats] = useState([]);
  const [selectedSeats, setSelectedSeats] = useState([]);
  const [sectors, setSectors] = useState([]); // Ahora los sectores vienen de la DB
  const [sectorId, setSectorId] = useState(null);
  const [isLogged, setIsLogged] = useState(!!localStorage.getItem("userId"));
  const [view, setView] = useState("login");

  // 1. CARGAR SECTORES DINÁMICAMENTE
  useEffect(() => {
    if (isLogged && eventId) {
      fetch(`http://localhost:5171/api/v1/events/${eventId}/sectors`)
        .then(res => res.json())
        .then(data => {
          setSectors(data);
          if (data && data.length > 0) {
            setSectorId(data[0].id); // Selecciona el primer sector automáticamente
          }
        })
        .catch(err => console.error("Error cargando sectores:", err));
    }
  }, [eventId, isLogged]);

  // 2. CARGAR ASIENTOS CUANDO CAMBIA EL SECTOR
  const loadSeats = () => {
    if (!sectorId) return;
    // URL Corregida para tu EventsController
    fetch(`http://localhost:5171/api/v1/events/${sectorId}/seats`)
      .then(res => res.json())
      .then(data => setSeats(data))
      .catch(err => console.error("Error cargando asientos:", err));
  };

  useEffect(() => {
    if (isLogged && sectorId) {
      loadSeats();
    }
  }, [sectorId, isLogged]);

  // --- LÓGICA DE SELECCIÓN (Igual a la que tenías) ---
  const toggleSeat = (seatId, status) => {
    if (status !== 'Available') return;
    setSelectedSeats(prev => 
      prev.includes(seatId) ? prev.filter(id => id !== seatId) : [...prev, seatId]
    );
  };

  const handleConfirm = async () => {
    const userId = localStorage.getItem("userId");
    if (selectedSeats.length === 0) return;
    try {
      const response = await fetch("http://localhost:5171/api/v1/reservations", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ userId: parseInt(userId), seatIds: selectedSeats })
      });
      if (response.ok) {
        alert("Reserva realizada con éxito");
        setSelectedSeats([]);
        loadSeats();
      } else {
        const text = await response.text();
        alert("Error al reservar: " + text);
      }
    } catch (error) {
      alert("Error de conexión");
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("userId");
    setIsLogged(false);
    setView("login");
    setSelectedSeats([]); 
  };

  if (!isLogged) {
    if (view === "login") {
      return <Login onLogin={() => setIsLogged(true)} goToRegister={() => setView("register")} />;
    }
    return <Register goToLogin={() => setView("login")} />;
  }

  return (
    <div style={{ padding: '20px', textAlign: 'center', backgroundColor: '#1a1a1a', color: '#ffffff', minHeight: '100vh' }}>
      
      <button onClick={() => navigate("/eventos")} style={{ position: 'absolute', top: '20px', left: '20px', padding: '8px 15px', backgroundColor: '#34495e', color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer' }}>
        ⬅ Volver
      </button>

      <button onClick={handleLogout} style={{ position: "absolute", top: "20px", right: "20px", padding: "8px 15px", backgroundColor: "#e74c3c", color: "white", border: "none", borderRadius: "5px", cursor: "pointer", fontWeight: 'bold' }}>
        Logout
      </button>

      <h1 style={{ color: '#ecf0f1', marginBottom: '30px' }}>Sistema de Ticketing</h1>

      {/* SECTORES DINÁMICOS: Se generan botones según lo que devuelva la API */}
      <div style={{ marginBottom: '20px', display: 'flex', gap: '15px', justifyContent: 'center', flexWrap: 'wrap' }}>
        {sectors.map(sector => (
          <button 
            key={sector.id}
            onClick={() => setSectorId(sector.id)}
            style={{
              padding: '10px 20px',
              backgroundColor: sectorId === sector.id ? '#3498db' : '#2c3e50',
              color: 'white',
              borderRadius: '5px',
              cursor: 'pointer',
              border: '1px solid #34495e',
              fontWeight: 'bold'
            }}>
            {sector.name}
          </button>
        ))}
      </div>

      <h3 style={{ color: '#bdc3c7' }}>
        {sectors.find(s => s.id === sectorId)?.name || 'Cargando mapa...'}
      </h3>

      <div style={{ marginBottom: '10px', fontSize: '1.1rem' }}>
        Asientos seleccionados: <strong style={{ color: '#3498db' }}>{selectedSeats.length}</strong>
      </div>

      <div style={{ 
        display: 'grid', 
        gridTemplateColumns: 'repeat(10, 45px)', 
        gap: '10px', 
        justifyContent: 'center',
        marginTop: '20px',
        backgroundColor: '#2c3e50',
        padding: '20px',
        borderRadius: '12px',
        boxShadow: '0 4px 15px rgba(0,0,0,0.3)'
      }}>
        {seats.length > 0 ? (
          seats.map(seat => (
            <div 
              key={seat.id}
              onClick={() => toggleSeat(seat.id, seat.status)}
              style={{
                width: '45px', height: '45px',
                backgroundColor: selectedSeats.includes(seat.id) ? '#3498db' : (seat.status === 'Available' ? '#27ae60' : '#c0392b'),
                color: 'white', display: 'flex', alignItems: 'center', justifyContent: 'center', borderRadius: '6px', fontSize: '12px', fontWeight: 'bold',
                cursor: seat.status === 'Available' ? 'pointer' : 'not-allowed',
                transition: '0.2s',
                transform: selectedSeats.includes(seat.id) ? 'scale(1.1)' : 'scale(1)',
                border: '1px solid rgba(255,255,255,0.1)'
              }}>
              {seat.seatNumber}
            </div>
          ))
        ) : (
          <p style={{ gridColumn: 'span 10' }}>No hay asientos disponibles en este sector.</p>
        )}
      </div>

      <div style={{ marginTop: '30px', display: 'flex', justifyContent: 'center', gap: '15px' }}>
        <button 
          disabled={selectedSeats.length === 0}
          onClick={() => setSelectedSeats([])}
          style={{ padding: '12px 25px', backgroundColor: selectedSeats.length > 0 ? '#e74c3c' : '#7f8c8d', color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer', fontWeight: 'bold' }}>
          Limpiar Selección
        </button>

        <button 
          disabled={selectedSeats.length === 0}
          onClick={handleConfirm}
          style={{ padding: '12px 30px', backgroundColor: selectedSeats.length > 0 ? '#2ecc71' : '#7f8c8d', color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer', fontWeight: 'bold' }}>
          Confirmar Reserva ({selectedSeats.length})
        </button>
      </div>
    </div>
  );
}

export default App;