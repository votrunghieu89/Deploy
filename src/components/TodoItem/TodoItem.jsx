import React from 'react';
import './TodoItem.css';

const TodoItem = ({ task, onToggle, onDelete }) => {
  const getStatusStyle = () => {
    switch (task.status) {
      case 'Quá hạn':
        return { color: '#e53e3e', backgroundColor: '#fff5f5' };
      case 'Đang thực hiện':
        return { color: '#d69e2e', backgroundColor: '#fffaf0' };
      case 'Hoàn thành':
        return { color: '#38a169', backgroundColor: '#f0fff4' };
      default:
        return {};
    }
  };

  const formatDate = (dateString) => {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleString('vi-VN');
  };

  return (
    <div className={`todo-item ${task.completed ? 'completed' : ''}`}>
      <div className="todo-item-header">
        <div className="checkbox-container">
          <input
            type="checkbox"
            checked={task.completed}
            onChange={() => onToggle(task.id)}
            className="todo-checkbox"
          />
        </div>
        <h3 className="todo-title">{task.title}</h3>
        <button 
          onClick={() => onDelete(task.id)}
          className="delete-button"
        >
          <span className="delete-icon">×</span>
        </button>
      </div>

      {task.description && (
        <p className="todo-description">{task.description}</p>
      )}

      <div className="todo-footer">
        {task.expireTime && (
          <span className="todo-date">
            {formatDate(task.expireTime)}
          </span>
        )}
        <span 
          className="todo-status"
          style={getStatusStyle()}
        >
          {task.status}
        </span>
      </div>
    </div>
  );
};

export default TodoItem;