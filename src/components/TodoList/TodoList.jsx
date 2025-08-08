import React from 'react';
import TodoItem from '../TodoItem/TodoItem';
import './TodoList.css';

const TodoList = ({ tasks, onToggle, onDelete }) => {
  if (tasks.length === 0) {
    return (
      <div className="empty-state">
        <p>Chưa có công việc nào</p>
      </div>
    );
  }

  return (
    <div className="todo-list">
      {tasks.map(task => (
        <TodoItem
          key={task.id}
          task={task}
          onToggle={onToggle}
          onDelete={onDelete}
        />
      ))}
    </div>
  );
};

export default TodoList;