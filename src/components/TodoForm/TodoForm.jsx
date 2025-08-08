import React, { useState } from 'react';
import './TodoForm.css';

const TodoForm = ({ onAddTask }) => {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [expireTime, setExpireTime] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!title.trim()) return;
    
    const newTask = {
      id: Date.now(),
      title: title.trim(),
      description: description.trim(),
      expireTime,
      completed: false,
      status: 'Đang thực hiện'
    };
    
    onAddTask(newTask);
    setTitle('');
    setDescription('');
    setExpireTime('');
  };

  return (
    <div className="todo-form-container">
      <h2 className="form-title">Thêm công việc mới</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label className="form-label">Tiêu đề*</label>
          <input
            type="text"
            className="form-input"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            placeholder="Nhập tiêu đề công việc..."
            required
          />
        </div>

        <div className="form-group">
          <label className="form-label">Mô tả</label>
          <textarea
            className="form-textarea"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            placeholder="Mô tả chi tiết công việc..."
            rows="3"
          />
        </div>

        <div className="form-group">
          <label className="form-label">Thời hạn</label>
          <input
            type="datetime-local"
            className="form-input"
            value={expireTime}
            onChange={(e) => setExpireTime(e.target.value)}
          />
        </div>

        <button type="submit" className="submit-button">
          Thêm công việc
        </button>
      </form>
    </div>
  );
};

export default TodoForm;