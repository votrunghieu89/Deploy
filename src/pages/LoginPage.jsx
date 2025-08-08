import React from 'react';
import LoginForm from '../components/LoginForm/LoginForm';
import './LoginPage.css';

const LoginPage = ({ onLogin, onRegister, onGoogleLogin }) => {
  return (
    <div className="login-page">
      <div className="login-container">
        <LoginForm 
          onLogin={onLogin}
          onRegister={onRegister}
          onGoogleLogin={onGoogleLogin}
        />
      </div>
    </div>
  );
};

export default LoginPage;