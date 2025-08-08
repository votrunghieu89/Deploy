import React, { useState, useEffect } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import TodoPage from './pages/TodoPage';
import './App.css';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem('accessToken'));
  const [userEmail, setUserEmail] = useState(localStorage.getItem('userEmail') || '');

  // Nhận message từ Google Login popup
  useEffect(() => {
    const handleMessage = (event) => {
      // Có thể check event.origin === 'https://localhost:7000' để bảo mật
      if (event.data && event.data.accessToken) {
        localStorage.setItem('accessToken', event.data.accessToken);
        localStorage.setItem('userEmail', event.data.email || 'user@gmail.com');
        setIsLoggedIn(true);
        setUserEmail(event.data.email || 'user@gmail.com');
      }
    };
    window.addEventListener('message', handleMessage);
    return () => window.removeEventListener('message', handleMessage);
  }, []);

  // Hàm login qua GraphQL
  const handleLogin = async (username, password) => {
    try {
      const response = await fetch('/api/graphql', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          query: `
            mutation {
              login(username: "${username}", password: "${password}") {
                accessToken
                userName
                email
                userId
              }
            }
          `,
        }),
      });

      const result = await response.json();

      if (result.errors) {
        alert('Tên đăng nhập hoặc mật khẩu không đúng!');
        return;
      }
      console.log('Status:', response.status);
      const { accessToken, email } = result.data.login;
      localStorage.setItem('accessToken', accessToken);
      localStorage.setItem('userEmail', email);
      setIsLoggedIn(true);
      setUserEmail(email);
    } catch (error) {
      console.error(error);
      alert('Lỗi kết nối đến máy chủ.');
    }
  };

  const handleRegister = () => {
    alert('Chức năng đăng ký sẽ được triển khai sau');
  };

  const handleLogout = () => {
    setIsLoggedIn(false);
    setUserEmail('');
    localStorage.removeItem('accessToken');
    localStorage.removeItem('userEmail');
  };

  return (
    <BrowserRouter>
      <div className="app">
        <Routes>
          <Route
            path="/"
            element={
              isLoggedIn ? (
                <Navigate to="/todopage" replace />
              ) : (
                <LoginPage
                  onLogin={handleLogin}
                  onRegister={handleRegister}
                />
              )
            }
          />
          <Route
            path="/todopage"
            element={
              localStorage.getItem('accessToken') ? (
                <TodoPage userEmail={userEmail} onLogout={handleLogout} />
              ) : (
                <Navigate to="/" replace />
              )
            }
          />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;
