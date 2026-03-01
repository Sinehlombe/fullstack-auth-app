import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { getUserDetails } from '../../api/auth'
import { useAuth } from '../../context/AuthContext'
import './UserDetails.css'

function UserDetails() {
  const navigate = useNavigate()
  const { logout } = useAuth()

  const [user, setUser] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await getUserDetails()
        setUser(response.data)
      } catch (err) {
        setError('Failed to load user details.')
      } finally {
        setLoading(false)
      }
    }

    fetchUser()
  }, [])

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  if (loading) return <div className="loading">Loading...</div>
  if (error) return <div className="error-message">{error}</div>

  return (
    <div className="details-container">
      <div className="details-card">
        <div className="details-header">
          <div className="avatar">
            {user.firstName?.charAt(0).toUpperCase()}
          </div>
          <h1>My Profile</h1>
        </div>

        <div className="details-body">
          <div className="detail-row">
            <span className="detail-label">First Name</span>
            <span className="detail-value">{user.firstName}</span>
          </div>

          <div className="detail-row">
            <span className="detail-label">Last Name</span>
            <span className="detail-value">{user.lastName}</span>
          </div>

          <div className="detail-row">
            <span className="detail-label">Email</span>
            <span className="detail-value">{user.email}</span>
          </div>
        </div>

        <button className="btn-logout" onClick={handleLogout}>
          Sign Out
        </button>
      </div>
    </div>
  )
}

export default UserDetails