<?php

namespace App\Models;

use App\Transformers\SettingTransformer;
use Custom;
use DotenvEditor;
use Eloquent;
use Flugg\Responder\Contracts\Transformable;
use Illuminate\Database\Eloquent\Builder;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Carbon;

/**
 * App\Models\Setting
 *
 * @property mixed $key
 * @property string|null $value
 * @property int $is_env
 * @property Carbon|null $created_at
 * @property Carbon|null $updated_at
 * @method static Builder|Setting newModelQuery()
 * @method static Builder|Setting newQuery()
 * @method static Builder|Setting query()
 * @method static Builder|Setting whereCreatedAt($value)
 * @method static Builder|Setting whereIsEnv($value)
 * @method static Builder|Setting whereKey($value)
 * @method static Builder|Setting whereUpdatedAt($value)
 * @method static Builder|Setting whereValue($value)
 * @mixin Eloquent
 */
class Setting extends Model implements Transformable
{
    /**
     * The primary key for the model.
     *
     * @var string
     */
    protected $primaryKey = 'key';

    /**
     * The "type" of the auto-incrementing ID.
     *
     * @var string
     */
    protected $keyType = 'varchar(255)';

    /**
     * The attributes that should be hidden for arrays.
     *
     * @var array
     */
    protected $hidden = [
        'is_env',
    ];

    /**
     * The "booting" method of the model.
     *
     * @return void
     */
    protected static function boot()
    {
        parent::boot();

        /*
         * Register a created model event with the dispatcher.
         *
         * @param \Closure|string $callback
         * @return void
         */
        self::creating(function ($model) {
            if ($model->is_env) {
                DotenvEditor::setKey(strtoupper($model->key), $model->value);
                DotenvEditor::save();
            }
        });

        /*
         * Register an updated model event with the dispatcher.
         *
         * @param \Closure|string $callback
         * @return void
         */
        self::updating(function ($model) {
            if ($model->is_env) {
                DotenvEditor::setKey(strtoupper($model->key), $model->value);
                DotenvEditor::save();
            }
        });
    }

    /**
     * @inheritDoc
     */
    public function transformer()
    {
        return SettingTransformer::class;
    }

    /**
     * Transform the value from database to the correct format.
     *
     * @return bool|mixed
     */
    public function decodeValue()
    {
        switch ($this->key) {
            case 'app_register':
            case 'homepage_enabled':
            case 'only_primary_domain':
            case 'unverified_user_alert':
                return (bool) $this->value;
                break;
            case 'app_icon':
                return [
                    'text' => $this->value,
                    'image' => Custom::getIcon(),
                ];
                break;
            case 'app_background':
                return [
                    'text' => $this->value,
                    'image' => Custom::getBackground(),
                ];
                break;
            default:
                return $this->value;
                break;
        }
    }

    /**
     * Transform the value from correct format to database.
     *
     * @param $value
     */
    public function encodeValue($value)
    {
        switch ($this->key) {
            case 'app_register':
            case 'homepage_enabled':
            case 'only_primary_domain':
            case 'unverified_user_alert':
                $this->value = (bool) $value;
                break;
            default:
                $this->value = $value;
                break;
        }
    }
}
