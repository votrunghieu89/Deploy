import React, { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import TodoForm from '../components/TodoForm/TodoForm';
import TodoList from '../components/TodoList/TodoList';
import './TodoPage.css';

const GRAPHQL_URL = '/api/graphql';

function getUserIdFromToken() {
  const token = localStorage.getItem('accessToken');
  if (!token) return null;
  try {
    const decoded = jwtDecode(token);
    return decoded.UserId || decoded.userId || decoded.userid || null;
  } catch {
    return null;
  }
}

const TodoPage = ({ onLogout }) => {
  const [tasks, setTasks] = useState([]);
  const [message, setMessage] = useState('');
  const userId = getUserIdFromToken();

  // Lấy toàn bộ task của user khi mount
  useEffect(() => {
    if (!userId) return;
    fetch(GRAPHQL_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
      },
      body: JSON.stringify({
        query: `
          query {
            getTasksByUserId(userId: ${userId}) {
              listID
              listTittle
              listDescription
              expriteTime
              isDone
            }
          }
        `,
      }),
    })
      .then(res => res.json())
      .then(data => {
        if (data.data && data.data.getTasksByUserId) {
          setTasks(
            data.data.getTasksByUserId.map(task => ({
              ...task,
              id: task.listID,
              title: task.listTittle,
              description: task.listDescription,
              expireTime: task.expriteTime,
              completed: task.isDone,
              status: task.isDone ? 'Hoàn thành' : 'Đang thực hiện',
            }))
          );
        }
      });
  }, [userId]);

  // Thêm task mới
  const addTask = async (newTask) => {
    if (!userId) return;
    // Đảm bảo expriteTime đúng định dạng ISO và có đuôi Z
    let expireTime = newTask.expireTime;
    if (expireTime && !expireTime.endsWith('Z')) {
      expireTime = new Date(expireTime).toISOString();
    }
    const response = await fetch(GRAPHQL_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
      },
      body: JSON.stringify({
        query: `
          mutation {
            createTask(task: {
              listTittle: "${newTask.title}",
              listDescription: "${newTask.description}",
              isDone: false,
              expriteTime: "${expireTime}",
              userId: ${userId}
            })
          }
        `,
      }),
    });
    if (response.ok) {
      setMessage('Đã thêm công việc thành công!');
      setTimeout(() => setMessage(''), 2000);
      // Reload tasks
      const res = await fetch(GRAPHQL_URL, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
        },
        body: JSON.stringify({
          query: `
            query {
              getTasksByUserId(userId: ${userId}) {
                listID
                listTittle
                listDescription
                expriteTime
                isDone
              }
            }
          `,
        }),
      });
      const data = await res.json();
      if (data.data && data.data.getTasksByUserId) {
        setTasks(
          data.data.getTasksByUserId.map(task => ({
            ...task,
            id: task.listID,
            title: task.listTittle,
            description: task.listDescription,
            expireTime: task.expriteTime,
            completed: task.isDone,
            status: task.isDone ? 'Hoàn thành' : 'Đang thực hiện',
          }))
        );
      }
    }
  };

  // Xóa task theo listID
  const deleteTask = async (taskId) => {
    const response = await fetch(GRAPHQL_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
      },
      body: JSON.stringify({
        query: `
          mutation {
            removeTask(taskId: ${taskId})
          }
        `,
      }),
    });
    if (response.ok) {
      setMessage('Đã xóa công việc thành công!');
      setTimeout(() => setMessage(''), 2000);
      setTasks(prevTasks => prevTasks.filter(task => task.id !== taskId));
    }
  };

  // Toggle hoàn thành (nếu có API thì gọi, không thì chỉ cập nhật local)
  const toggleTask = (taskId) => {
    setTasks(prevTasks =>
      prevTasks.map(task =>
        task.id === taskId
          ? {
              ...task,
              completed: !task.completed,
              status: !task.completed ? 'Hoàn thành' : 'Đang thực hiện',
            }
          : task
      )
    );
    // Có thể gọi API updateTask nếu backend hỗ trợ
  };

  return (
    <div className="todo-page">
      <header className="todo-header">
        <h1>Công việc của tôi</h1>
        <button onClick={onLogout} className="logout-button">
          Đăng xuất
        </button>
      </header>

      {message && <div className="todo-message">{message}</div>}

      <main className="todo-main">
        <TodoForm onAddTask={addTask} />
        <TodoList
          tasks={tasks}
          onToggle={toggleTask}
          onDelete={deleteTask}
        />
      </main>
    </div>
  );
};

export default TodoPage;