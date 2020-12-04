<?php

namespace App\Models;

use App\Transformers\UserTransformer;
use Avatar;
use Eloquent;
use EloquentFilter\Filterable;
use Flugg\Responder\Contracts\Transformable;
use Illuminate\Database\Eloquent\Builder;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Foundation\Auth\User as Authenticatable;
use Illuminate\Notifications\DatabaseNotification;
use Illuminate\Notifications\DatabaseNotificationCollection;
use Illuminate\Notifications\Notifiable;
use Illuminate\Support\Carbon;
use Storage;
use Tymon\JWTAuth\Contracts\JWTSubject;

/**
 * App\Models\User
 *
 * @property int $id
 * @property string $name
 * @property string|null $surname
 * @property string $email
 * @property Carbon|null $email_verified_at
 * @property string $password
 * @property string|null $avatar
 * @property int $role_id
 * @property int $status
 * @property string|null $remember_token
 * @property Carbon|null $created_at
 * @property Carbon|null $updated_at
 * @property-read DatabaseNotificationCollection|DatabaseNotification[] $notifications
 * @property-read int|null $notifications_count
 * @property-read Role $role
 * @method static Builder|User filter($input = [], $filter = null)
 * @method static Builder|User newModelQuery()
 * @method static Builder|User newQuery()
 * @method static Builder|User paginateFilter($perPage = null, $columns = [], $pageName = 'page', $page = null)
 * @method static Builder|User query()
 * @method static Builder|User simplePaginateFilter($perPage = null, $columns = [], $pageName = 'page', $page = null)
 * @method static Builder|User whereAvatar($value)
 * @method static Builder|User whereBeginsWith($column, $value, $boolean = 'and')
 * @method static Builder|User whereCreatedAt($value)
 * @method static Builder|User whereEmail($value)
 * @method static Builder|User whereEmailVerifiedAt($value)
 * @method static Builder|User whereEndsWith($column, $value, $boolean = 'and')
 * @method static Builder|User whereId($value)
 * @method static Builder|User whereLike($column, $value, $boolean = 'and')
 * @method static Builder|User whereName($value)
 * @method static Builder|User wherePassword($value)
 * @method static Builder|User whereRememberToken($value)
 * @method static Builder|User whereRoleId($value)
 * @method static Builder|User whereStatus($value)
 * @method static Builder|User whereSurname($value)
 * @method static Builder|User whereUpdatedAt($value)
 * @mixin Eloquent
 */
class User extends Authenticatable implements Transformable, JWTSubject
{
    use Filterable;
    use Notifiable;

    /**
     * The attributes that should be hidden for arrays.
     *
     * @var array
     */
    protected $hidden = [
        'password', 'remember_token',
    ];

    /**
     * The attributes that should be cast to native types.
     *
     * @var array
     */
    protected $casts = [
        'email_verified_at' => 'datetime',
    ];

    /**
     * Get the identifier that will be stored in the subject claim of the JWT.
     *
     * @return mixed
     */
    public function getJWTIdentifier()
    {
        return $this->getKey();
    }

    /**
     * Return a key value array, containing any custom claims to be added to the JWT.
     *
     * @return array
     */
    public function getJWTCustomClaims()
    {
        return [];
    }

    /**
     * @inheritDoc
     */
    public function transformer()
    {
        return UserTransformer::class;
    }

    /**
     * Method to check if the user has been verified.
     *
     * @return bool
     */
    public function isVerified()
    {
        if ($this->email_verified_at == null) {
            return false;
        } else {
            return true;
        }
    }

    /**
     * Return user data
     *
     * @return BelongsTo
     */
    public function role()
    {
        return $this->belongsTo('App\Models\Role');
    }

    /**
     * Get´s the user avatar.
     *
     * @return string
     */
    public function getAvatar()
    {
        $defaultAvatar = Avatar::create($this->fullName())->toBase64();
        switch ($this->avatar) {
            case null:
            case 'default':
                return $defaultAvatar;
                break;
            default:
                if (Storage::disk('public')->exists($this->avatar)) {
                    return url(Storage::disk('public')->url($this->avatar));
                } else {
                    return $defaultAvatar;
                }
        }
    }

    /**
     * Get user fullName
     *
     * @return string
     */
    public function fullName()
    {
        if (!empty($this->surname)) {
            return $this->name.' '.$this->surname;
        }
        return $this->name;
    }
}
