import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import './index.css'
import App from './App.jsx'
import Login from './Login.jsx'
import Register from './Register.jsx'
import AdminDashboard from './AdminDashboard.jsx'
import EventList from './EventList.jsx' 

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login onLogin={() => {}} />} />
        <Route path="/register" element={<Register />} />
        <Route path="/eventos" element={<EventList />} /> 
        <Route path="/reserva/:eventId" element={<App />} /> 
        <Route path="/admin" element={<AdminDashboard />} />
      </Routes>
    </BrowserRouter>
  </StrictMode>,
)