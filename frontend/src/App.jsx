import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import Register from './pages/Register/Register'
import Login from './pages/Login/Login'
import UserDetails from './pages/UserDetails/UserDetails'
import PrivateRoute from './components/PrivateRoute'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Navigate to="/login" />} />
        <Route path="/register" element={<Register />} />
        <Route path="/login" element={<Login />} />
        <Route path="/user" element={
          <PrivateRoute>
            <UserDetails />
          </PrivateRoute>
        } />
      </Routes>
    </BrowserRouter>
  )
}

export default App