<?php

namespace App\Transformers;

use App\Models\User;
use Flugg\Responder\Transformers\Transformer;

class UserTransformer extends Transformer
{
    /**
     * List of available relations.
     *
     * @var string[]
     */
    protected $relations = [];

    /**
     * List of autoloaded default relations.
     *
     * @var array
     */
    protected $load = [];

    /**
     * Transform the model.
     *
     * @param  User  $user
     * @return array
     */
    public function transform(User $user)
    {
        return [
            'id' => (int) $user->id,
            'name' => (string) $user->name,
            'surname' => (string) $user->surname,
            'full_name' => (string) $user->fullName(),
            'email' => (string) $user->email,
            'avatar' => (string) $user->getAvatar(),
            'role' => (int) $user->role_id,
            'role_details' => $user->role,
            'permissions' => $user->role->getPermissions(),
            'verified' => (bool) $user->isVerified(),
            'status' => (bool) $user->status,
        ];
    }
}
