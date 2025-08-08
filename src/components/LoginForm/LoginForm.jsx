import React, { useState } from 'react';
import './LoginForm.css';
import GoogleLoginButton from '../GoogleLoginButton/GoogleLoginButton';

const LoginForm = ({ onRegister }) => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    try {
      const response = await fetch('/api/graphql', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
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
        setError('Tên đăng nhập hoặc mật khẩu không đúng!');
        return;
      }

      const { accessToken } = result.data.login;
      localStorage.setItem('accessToken', accessToken);
      window.location.href = '/todopage'; // Redirect

    } catch (err) {
      console.error(err);
      setError('Lỗi kết nối đến máy chủ.');
    }
  };

  const handleGoogleLogin = () => {
    const popup = window.open(
      'https://localhost:7000/api/Google/login',
      'GoogleLogin',
      'width=500,height=600'
    );

    const popupTick = setInterval(() => {
      if (popup.closed) {
        clearInterval(popupTick);
      }
    }, 500);
  };

  return (
    <div className="login-form-container">
      <h1 className="login-title">Todo App</h1>
      <p className="login-subtitle">Quản lý công việc hiệu quả</p>

      <form className="login-form" onSubmit={handleSubmit}>
        <div className="form-group">
          <label className="form-label">Tên đăng nhập</label>
          <input
            type="text"
            className="form-input"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            placeholder="Nhập tên đăng nhập"
          />
        </div>

        <div className="form-group">
          <label className="form-label">Mật khẩu</label>
          <input
            type="password"
            className="form-input"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Nhập mật khẩu"
          />
        </div>

        {error && <p className="error-text">{error}</p>}

        <div className="form-actions">
          <button type="submit" className="login-button">
            Đăng nhập
          </button>
          <button
            type="button"
            className="register-button"
            onClick={onRegister}
          >
            Đăng ký
          </button>
        </div>

        <div className="divider">
          <span className="divider-text">Hoặc</span>
        </div>

        <GoogleLoginButton onClick={handleGoogleLogin} />

        <p className="terms-text">
          Bằng cách đăng nhập, bạn đồng ý với điều khoản sử dụng
        </p>
      </form>
    </div>
  );
};

export default LoginForm;
